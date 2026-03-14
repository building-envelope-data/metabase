#!/usr/bin/env bash
# [Bash Strict Mode](https://github.com/olivergondza/bash-strict-mode)
set -o errexit
set -o errtrace
set -o nounset
set -o pipefail
# shellcheck disable=SC2154 # warning: s is referenced but not assigned.
trap 'status_code=$?; echo "$0: Error on line "${LINENO}": ${BASH_COMMAND}" >&2; exit $status_code' ERR

INVOCATION=$(printf "%q " "$0" "$@")

#-----------------------------------------------
# COLORS

RED=$(tput setaf 1)
GREEN=$(tput setaf 2)
YELLOW=$(tput setaf 3)
RESET=$(tput sgr0)

#-----------------------------------------------
# GLOBAL VARIABLES

COMMAND=
TARGET=
RESTORE_TIMESTAMP=
ON_ERROR=pause
DRY_RUN=false
STEP=initialize
HISTORY_PATH=./.deploy-history
touch "${HISTORY_PATH}"

#-----------------------------------------------
# USAGE

usage() {
  local script_name
  script_name=$(basename "$0")
  cat <<EOF

${script_name} - Deployment Util

Deploy a Git target, restore a previous deployment, resume or rollback a paused
deploy and restore attempt, print the current state, or list previous
deployments.

USAGE:
  ${script_name} target <GIT_TARGET> [options]
  ${script_name} restore <TIMESTAMP> [options]
  ${script_name} resume [options]
  ${script_name} rollback [options]
  ${script_name} state
  ${script_name} list
  ${script_name} --help

OPTIONS:
  -e, --on-error <ACTION>   When an error occurs then: pause, rollback, or ask (for user input). (default: pause)
  -d, --dry-run             Print commands instead of running them.
  -h, --help                Display this help message.

EXAMPLES:
  ${script_name} target v1.0.0 --on-error ask
  ${script_name} restore 2026-03-12T21:43:11+01:00
  ${script_name} resume
  ${script_name} rollback --dry-run
  ${script_name} state
  ${script_name} list | less
EOF
  exit 1
}

#-----------------------------------------------
# ARGUMENTS & OPTIONS

[[ $# -eq 0 ]] && usage

COMMAND="$1"

case "${COMMAND}" in
target)
  COMMAND=deploy
  if [[ ! -n "${2-}" ]]; then
    echo "${RED}[Error]${RESET} Git target is missing" >&2
    exit 1
  fi
  TARGET="$2"
  shift 2 # skip command and target
  ;;
restore)
  if [[ ! -n "${2-}" ]]; then
    echo "${RED}[Error]${RESET} Timestamp is missing" >&2
    exit 1
  fi
  RESTORE_TIMESTAMP="$2"
  shift 2 # skip command and timestamp
  ;;
resume | rollback | state | list)
  shift 1 # skip command
  ;;
--help)
  usage
  ;;
*)
  echo "${RED}[Error]${RESET} Unsupported command '${COMMAND}'." >&2
  usage
  ;;
esac

while [[ $# -gt 0 ]]; do
  case "$1" in
  -e | --on-error)
    if [[ -z "$2" ]]; then
      echo "${RED}[Error]${RESET} --on-error requires a value" >&2
      exit 1
    fi
    ON_ERROR="$2"
    shift 2 # skip name and value
    ;;
  -d | --dry-run)
    DRY_RUN=true
    shift # skip to the next option
    ;;
  -h | --help)
    usage
    ;;
  *)
    echo "${RED}[Error]${RESET} Unknown option '$1'" >&2
    usage
    ;;
  esac
done

case "${ON_ERROR}" in
pause | restore | ask) ;;
*)
  echo "${RED}[Error]${RESET} --on-error is neither 'pause' nor 'restore' nor 'ask' but '${ON_ERROR}'." >&2
  usage
  ;;
esac

#-----------------------------------------------
# DRY-RUN

run() {
  if ${DRY_RUN}; then
    echo "${YELLOW}[DRY-RUN]${RESET} Would execute: $*" >&2
  else
    "$@"
  fi
}

#-----------------------------------------------
# ATTEMPT

declare -A attempt=() # associative array

read_last_attempt_matching() {
  local pattern="$1"
  echo "Reading last attempt from history matching '${pattern}'" >&2
  local line
  line=$(tac "${HISTORY_PATH}" 2>/dev/null | grep --max-count=1 "${pattern}" || true)
  if [[ -n "${line}" ]]; then
    IFS=',' read -ra pairs <<<"${line}"
    for pair in "${pairs[@]}"; do
      key="${pair%%=*}"
      value="${pair#*=}"
      attempt["${key}"]="${value}"
    done
  fi
}

append_or_overwrite_attempt() {
  echo "Writing attempt to history '${HISTORY_PATH}'" >&2
  entries=()
  for key in "${!attempt[@]}"; do
    entries+=("$key=${attempt[$key]}")
  done
  line="$(
    IFS=,
    echo "${entries[*]}"
  )"
  if [[ "${COMMAND}" == "resume" ]]; then
    run sed --in-place "\$c ${line}" "${HISTORY_PATH}"
  else
    run echo "${line}" >>"${HISTORY_PATH}"
  fi
}

prepare_attempt() {
  local now
  local default_backup_dir
  now="$(date --iso-8601=seconds)"
  default_backup_dir="/app/data/backups/$(date +"%Y-%m-%d_%H_%M_%S")"
  case "${COMMAND}" in
  deploy)
    attempt["command"]="${COMMAND}"
    attempt["timestamp"]="${now}"
    attempt["backup_dir"]="${default_backup_dir}"
    attempt["target"]="${TARGET}"
    ;;
  restore)
    read_last_attempt_matching "timestamp=${RESTORE_TIMESTAMP}"
    if [[ ${#attempt[@]} -eq 0 ]]; then
      echo "There is no deployment with timestamp '${RESTORE_TIMESTAMP}' to ${COMMAND}. To print all available deployments run \`./deploy.sh list\`" >&2
      exit 1
    fi
    attempt["command"]="${COMMAND}"
    attempt["timestamp"]="${now}"
    attempt["restore_dir"]="${attempt["backup_dir"]-}"
    attempt["backup_dir"]="${default_backup_dir}"
    attempt["restore_timestamp"]="${RESTORE_TIMESTAMP}"
    ;;
  resume | rollback)
    read_last_attempt_matching "." # read last attempt
    # note that `"${COMMAND}"` and `attempt["command"]` differ in these cases
    if [[ ${#attempt[@]} -eq 0 ]]; then
      echo "There is no paused deploy or restore attempt to ${COMMAND}." >&2
      exit 1
    fi
    ;;
  state)
    read_last_attempt_matching "." # read last attempt
    # note that `"${COMMAND}"` and `attempt["command"]` differ in this case
    ;;
  list) ;;
  *)
    echo "${RED}[Error]${RESET} Unsupported command '${COMMAND}'." >&2
    usage
    ;;
  esac
}

prepare_attempt

#===============================================
# ACT

case "${COMMAND}" in

list)
  tac "${HISTORY_PATH}"
  ;;

state)
  if [[ ${#attempt[@]} -eq 0 ]]; then
    echo "No deployment attempt has been made yet."
  else
    if [[ ! -v attempt["until"] ]]; then
      echo "The last deployment attempt succeeded." >&2
    else
      case "${attempt["command"]-}" in
      deploy)
        echo "Paused deploy of '${attempt["target"]-}' at step '${attempt["until"]-}'. Resume or rollback." >&2
        ;;
      restore)
        echo "Paused restore of '${attempt["restore_timestamp"]-}' at step '${attempt["until"]-}'. Resume or rollback." >&2
        ;;
      *)
        echo "Unsupported command '${attempt["command"]-}'" >&2
        ;;
      esac
    fi
  fi
  ;;

deploy | restore | resume)

  #===============================================
  # DEPLOY OR RESTORE OR RESUME

  #-----------------------------------------------
  # CLEANUP

  cleanup_deploy_or_restore_or_resume() {
    local exit_code="$1"
    local line_number="$2"
    local bash_command="$3"
    [ "${exit_code}" -eq 0 ] && exit 0 # exit normally if no error

    echo >&2
    echo "${RED}[Error]${RESET} Failed during step '${STEP-unknown}' with exit code '${exit_code}' on line '${line_number}' running '${bash_command}'" >&2

    append_or_overwrite_attempt

    pause() {
      echo "Pausing. Fix the issue. Then resume with \`./deploy.sh resume\` or rollback wtih \`./deploy.sh rollback\`"
      exit "${exit_code}"
    }

    rollback() {
      echo "Rolling back. Afterwards fix the issue. Then retry with \`${INVOCATION}\`"
      run ./deploy.sh rollback
      exit "${exit_code}"
    }

    case "${ON_ERROR}" in
    pause)
      pause
      ;;
    rollback)
      rollback
      ;;
    ask | *)
      while true; do
        read -rp "Do you want to [p]ause or [r]estore? " action
        case "${action,,}" in
        p | pause) pause ;;
        r | rollback) rollback ;;
        *) echo "Invalid choice. Please type 'p' for pause or 'r' for rollback." ;;
        esac
      done
      ;;
    esac
  }

  # Trap all exits (errors or manual cancels)
  trap 'cleanup_deploy_or_restore_or_resume $? ${LINENO} ${BASH_COMMAND}' EXIT

  #-----------------------------------------------
  # DO

  case "${COMMAND}" in
  deploy)
    echo "Deploying target '${attempt["target"]-}'" >&2
    ;;
  restore)
    echo "Restoring deployment '${attempt["restore_timestamp"]-}'" >&2
    ;;
  resume)
    case "${attempt["command"]-}" in
    deploy)
      echo "Resuming deploy of '${attempt["target"]-}' at step '${attempt["until"]-}'" >&2
      ;;
    restore)
      echo "Resuming restore of '${attempt["restore_timestamp"]-}' at step '${attempt["until"]-}'" >&2
      ;;
    *)
      echo "${RED}[Error]${RESET} Unsupported command '${attempt["command"]-}'" >&2
      exit 1
      ;;
    esac
    ;;
  *)
    echo "${RED}[Error]${RESET} Unsupported command '${COMMAND}'" >&2
    exit 1
    ;;
  esac

  case "${attempt["until"]-"begin-maintenance"}" in
  *) # run always
    STEP="begin-maintenance"
    attempt["until"]=${STEP}
    echo "${GREEN}[Step]${RESET} Beginning maintenance mode" >&2
    run ./deploy.mk begin-maintenance || exit 1
    ;;&                 # continue with a proper match below
  begin-maintenance) ;& # fall through
  set-target)
    STEP="set-target"
    attempt["until"]=${STEP}
    echo "${GREEN}[Step]${RESET} Setting target in ./.env to '${attempt["target"]-}'" >&2
    attempt["previous_target"]="$(grep --only-matching --perl-regexp '(?<=TARGET=).*' ./.env)"
    [[ -z "${attempt[previous_target]}" ]] && (
      echo "${RED}[Error]${RESET} Previous target is missing in ./.env" >&2
      exit 1
    )
    [[ -z "${attempt["target"]-}" ]] && (
      echo "${RED}[Error]${RESET} Target is unknown" >&2
      exit 1
    )
    run ./deploy.mk set-target TARGET="${attempt["target"]-}" || exit 1
    ;& # fall through
  backup)
    STEP="backup"
    attempt["until"]=${STEP}
    echo "${GREEN}[Step]${RESET} Backing up data into '${attempt["backup_dir"]-}'" >&2
    [[ -z "${attempt["backup_dir"]-}" ]] && (
      echo "${RED}[Error]${RESET} Backup directory is unkown" >&2
      exit 1
    )
    run ./database.mk backup DIR="${attempt["backup_dir"]-}" || exit 1
    ;& # fall through
  switch)
    STEP="switch"
    attempt["until"]=${STEP}
    echo "${GREEN}[Step]${RESET} Fetching code from Git remote and switching to Git target '${attempt["target"]-}'" >&2
    [[ -z "${attempt["target"]-}" ]] && (
      echo "${RED}[Error]${RESET} Target is unknown" >&2
      exit 1
    )
    run ./deploy.mk fetch-all || exit 1
    run ./deploy.mk switch TARGET="${attempt["target"]-}" || exit 1
    ;& # fall through
  dotenv)
    STEP="dotenv"
    attempt["until"]=${STEP}
    echo "${GREEN}[Step]${RESET} Checking dotenv file ./.env for compatibility with ./.env.production.yaml" >&2
    run ./deploy.mk dotenv || exit 1
    ;& # fall through
  migrate-or-restore)
    STEP="migrate-or-restore"
    attempt["until"]=${STEP}
    case "${attempt["command"]-}" in
    deploy)
      echo "${GREEN}[Step]${RESET} Migrating PostgreSQL database" >&2
      run ./database.mk migrate || exit 1
      ;;
    restore)
      echo "${GREEN}[Step]${RESET} Restoring data" >&2
      [[ -z "${attempt["restore_dir"]-}" ]] && (
        echo "${RED}[Error]${RESET} Restore directory is unkown" >&2
        exit 1
      )
      run ./database.mk restore DIR="${attempt["restore_dir"]-}" || exit 1
      ;;
    *)
      echo "${RED}[Error]${RESET} Unsupported command '${attempt["command"]-}'" >&2
      exit 1
      ;;
    esac
    ;& # fall through
  services)
    STEP="services"
    attempt["until"]=${STEP}
    echo "${GREEN}[Step]${RESET} Recreating Docker Compose services" >&2
    run ./deploy.mk services || exit 1
    ;& # fall through
  run-tests)
    STEP="run-tests"
    attempt["until"]=${STEP}
    echo "${GREEN}[Step]${RESET} Running tests" >&2
    run ./deploy.mk run-tests || exit 1
    ;& # fall through
  end-maintenance)
    STEP="end-maintenance"
    attempt["until"]=${STEP}
    echo "${GREEN}[Step]${RESET} Ending maintenance mode" >&2
    run ./deploy.mk end-maintenance || exit 1
    ;;
  esac

  unset "attempt[until]"
  append_or_overwrite_attempt

  echo "Done :)" >&2

  ;;

rollback)

  #===============================================
  # ROLLBACK

  #-----------------------------------------------
  # CLEANUP
  cleanup_rollback() {
    local exit_code="$1"
    local line_number="$2"
    local bash_command="$3"
    [ "${exit_code}" -eq 0 ] && exit 0 # exit normally if no error

    echo >&2
    echo "${RED}[Error]${RESET} Failed rolling back step '${STEP-unknown}' with exit code '${exit_code}' on line '${line_number}' running '${bash_command}'" >&2
    echo "Aborting. Fix the issue. Then retry with \`${INVOCATION}\`"
    exit "${exit_code}"
  }
  # Trap all exits (errors or manual cancels)
  trap 'cleanup_rollback $? ${LINENO} ${BASH_COMMAND}' EXIT

  #-----------------------------------------------
  # DO
  echo "Rolling back to target '${attempt[previous_target]}'" >&2

  rollback_steps() {
    STEP="begin-maintenance"
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0
    echo "${GREEN}[Step]${RESET} Beginning maintenance mode" >&2
    run ./deploy.mk begin-maintenance || exit 1

    STEP="set-target"
    echo "${GREEN}[Step]${RESET} Setting target in ./.env to '${attempt[previous_target]-}'" >&2
    [[ -z "${attempt[previous_target]-}" ]] && (
      echo "${RED}[Error]${RESET} Previous target is unknown" >&2
      exit 1
    )
    run ./deploy.mk set-target TARGET="${attempt[previous_target]-}" || exit 1
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    STEP="backup"
    # keep the backup for the step `migrate-or-restore`
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    STEP="switch"
    echo "${GREEN}[Step]${RESET} Fetching code from Git remote and switching to Git target '${attempt[previous_target]-}'" >&2
    [[ -z "${attempt[previous_target]-}" ]] && (
      echo "${RED}[Error]${RESET} Previous target is unknown" >&2
      exit 1
    )
    run ./deploy.mk fetch-all || exit 1
    run ./deploy.mk switch TARGET="${attempt[previous_target]-}" || exit 1
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    STEP="dotenv"
    echo "${GREEN}[Step]${RESET} Checking dotenv file ./.env for compatibility with ./.env.production.yaml" >&2
    run ./deploy.mk dotenv || exit 1
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    STEP="migrate-or-restore"
    echo "${GREEN}[Step]${RESET} Restoring data from '${attempt["backup_dir"]-}'" >&2
    [[ -z "${attempt["backup_dir"]-}" ]] && (
      echo "${RED}[Error]${RESET} Backup directory is unkown" >&2
      exit 1
    )
    run ./database.mk restore DIR="${attempt["backup_dir"]-}" || exit 1
    # re-create services just to make sure instead of `[[ "${attempt["until"]}" == "${STEP}" ]] && return 0`

    STEP="services"
    echo "${GREEN}[Step]${RESET} Recreating Docker Compose services" >&2
    run ./deploy.mk services || exit 1
    # run tests just to make sure instead of `[[ "${attempt["until"]}" == "${STEP}" ]] && return 0`

    STEP="run-tests"
    echo "${GREEN}[Step]${RESET} Running tests" >&2
    run ./deploy.mk run-tests || exit 1
    # [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    return 0
  }
  rollback_steps

  STEP="backup"
  echo "${GREEN}[Step]${RESET} Removing data backup '${attempt["backup_dir"]-}'" >&2
  rm --recursive --force "${attempt["backup_dir"]-}" || exit 1

  STEP="end-maintenance"
  echo "${GREEN}[Step]${RESET} Ending maintenance mode" >&2
  run ./deploy.mk end-maintenance || exit 1

  echo "Done :)" >&2

  ;;

esac
