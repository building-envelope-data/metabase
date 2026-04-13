#!/usr/bin/env bash
# [Bash Strict Mode](https://github.com/olivergondza/bash-strict-mode)
set -o errexit
set -o errtrace
set -o nounset
set -o pipefail
# set -o functrace # ensures DEBUG trap works inside functions
# shellcheck disable=SC2154 # warning: s is referenced but not assigned.
trap 'status_code=$?; echo "$0: Error on line "${LINENO}": ${BASH_COMMAND}" >&2; exit ${status_code}' ERR
# capture last line number (needed when script is exited via `exit ...` instead of an error)
trap 'LAST_LINENO=${LINENO}' ERR # DEBUG
LAST_LINENO=-1

INVOCATION=$(printf "%q " "$0" "$@")

#-----------------------------------------------
# COLORS & FORMATTING

RED=$(tput setaf 1)
GREEN=$(tput setaf 2)
YELLOW=$(tput setaf 3)
BLUE=$(tput setaf 4)
CYAN=$(tput setaf 6)
BOLD=$(tput bold)
DIM=$(tput setaf 244) # `tput dim` is considered a legacy feature by some, for example, kitty does not support it
RESET=$(tput sgr0)

#-----------------------------------------------
# ICONS & SYMBOLS
CHECK="✓"
CROSS="✗"
# ARROW="➜"
WARN="▲" # ⚠
BULLET="•"

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
# OUTPUT HELPERS

print_header() {
  local title="$1"
  echo "${BOLD}${CYAN}···························································${RESET}" >&2
  echo "${BOLD}${CYAN}${title}${RESET}" >&2
  echo "${BOLD}${CYAN}···························································${RESET}" >&2
}

print_section() {
  local title="$1"
  echo >&2
  echo "${BOLD}${BLUE}${title}${RESET}" >&2
  echo "${BLUE}· · · · · · · · · · · · · · · · · · · · · · · · · · · · · ·${RESET}" >&2
}

print_success() {
  local message="$1"
  echo "${GREEN}${CHECK}${RESET} ${message}" >&2
}

print_error() {
  local message="$1"
  echo "${RED}${CROSS}${RESET} ${RED}${BOLD}${message}${RESET}" >&2
}

print_warning() {
  local message="$1"
  echo "${YELLOW}${WARN}${RESET} ${YELLOW}${message}${RESET}" >&2
}

print_info() {
  local message="$1"
  echo "${CYAN}${BULLET}${RESET} ${message}" >&2
}

print_step() {
  local step_num="$1"
  local total_steps="$2"
  local step_name="$3"
  echo "${GREEN}[${step_num}/${total_steps}]${RESET} ${step_name}" >&2
}

#-----------------------------------------------
# USAGE

usage() {
  local script_name
  script_name=$(basename "$0")
  cat <<EOF

${BOLD}${CYAN}${script_name}${RESET} - ${BOLD}Deployment Utility${RESET}

${DIM}Deploy a Git target, restore a previous deployment, resume or rollback a paused
deploy and restore attempt, print the current state, or list previous deployments.${RESET}

${BOLD}USAGE:${RESET}
  ${CYAN}${script_name} target${RESET} <GIT_TARGET> [options]
  ${CYAN}${script_name} restore${RESET} <TIMESTAMP> [options]
  ${CYAN}${script_name} resume${RESET} [options]
  ${CYAN}${script_name} rollback${RESET} [options]
  ${CYAN}${script_name} state${RESET}
  ${CYAN}${script_name} list${RESET}
  ${CYAN}${script_name} --help${RESET}

${BOLD}OPTIONS:${RESET}
  ${GREEN}-e, --on-error${RESET} <ACTION>   When an error occurs then: ${DIM}pause${RESET}, ${DIM}rollback${RESET}, or ${DIM}ask${RESET} (for user input). ${DIM}(default: pause)${RESET}
  ${GREEN}-d, --dry-run${RESET}             Print commands instead of running them.
  ${GREEN}-h, --help${RESET}                Display this help message.

${BOLD}EXAMPLES:${RESET}
  ${DIM}${script_name} target v1.0.0 --on-error ask${RESET}
  ${DIM}${script_name} restore 2026-03-12T21:43:11+01:00${RESET}
  ${DIM}${script_name} resume${RESET}
  ${DIM}${script_name} rollback --dry-run${RESET}
  ${DIM}${script_name} state${RESET}
  ${DIM}${script_name} list | less${RESET}

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
  if [[ -z "${2-}" ]]; then
    print_error "Git target is missing"
    exit 1
  fi
  TARGET="$2"
  shift 2 # skip command and target
  ;;
restore)
  if [[ -z "${2-}" ]]; then
    print_error "Timestamp is missing"
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
  print_error "Unsupported command '${COMMAND}'."
  usage
  ;;
esac

while [[ $# -gt 0 ]]; do
  case "$1" in
  -e | --on-error)
    if [[ -z "$2" ]]; then
      print_error "--on-error requires a value"
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
    print_error "Unknown option '$1'"
    usage
    ;;
  esac
done

case "${ON_ERROR}" in
pause | restore | ask) ;;
*)
  print_error "--on-error is neither 'pause' nor 'restore' nor 'ask' but '${ON_ERROR}'."
  usage
  ;;
esac

#-----------------------------------------------
# DRY-RUN

run() {
  if ${DRY_RUN}; then
    echo "${YELLOW}[DRY-RUN]${RESET} Would execute: ${BOLD}$*${RESET}" >&2
  else
    "$@"
  fi
}

fetch_target_from_dotenv() {
  grep --only-matching --perl-regexp '(?<=TARGET=).*' ./.env
}

#-----------------------------------------------
# ATTEMPT

declare -A attempt=() # associative array

read_last_attempt_matching() {
  local pattern="$1"
  print_info "Reading last attempt matching '${pattern}' from history '${HISTORY_PATH}'"
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
  print_info "Writing attempt '${attempt["timestamp"]}' to history '${HISTORY_PATH}'"
  entries=("timestamp=${attempt["timestamp"]}")
  unset "attempt[timestamp]"
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

remove_attempt() {
  print_info "Removing attempt '${attempt["timestamp"]}' from history"
  run sed --in-place "/timestamp=${attempt["timestamp"]}/d" "${HISTORY_PATH}"
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
    read_last_attempt_matching "^timestamp=${RESTORE_TIMESTAMP},"
    if [[ ${#attempt[@]} -eq 0 ]]; then
      print_error "There is no deployment with timestamp '${RESTORE_TIMESTAMP}' to restore."
      print_info "To print all available deployments run \`${CYAN}./deploy.sh list${RESET}\`"
      exit 1
    fi
    if [[ -n "${attempt["until"]-}" ]]; then
      print_error "The deployment with timestamp '${RESTORE_TIMESTAMP}' did not succeed but failed at step '${attempt["until"]}'."
      print_info "You cannot restore a failed deployment. To print all available deployments run \`${CYAN}./deploy.sh list${RESET}\`"
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
    if [[ ${#attempt[@]} -eq 0 ]] || [[ -z "${attempt["until"]-}" ]]; then
      print_error "There is no paused deploy or restore attempt to ${COMMAND}."
      exit 1
    fi
    ;;
  state)
    read_last_attempt_matching "." # read last attempt
    # note that `"${COMMAND}"` and `attempt["command"]` differ in this case
    ;;
  list) ;;
  *)
    print_error "Unsupported command '${COMMAND}'."
    usage
    ;;
  esac
}

prepare_attempt

#===============================================
# ACT

case "${COMMAND}" in

list)
  print_header "HISTORY"
  # IFS= prevents whitespace trimming and -r backslash escaping
  tac "${HISTORY_PATH}" | while IFS= read -r line; do
    attempt=()
    if [[ -n "${line}" ]]; then
      IFS=',' read -ra pairs <<<"${line}"
      for pair in "${pairs[@]}"; do
        key="${pair%%=*}"
        value="${pair#*=}"
        attempt["${key}"]="${value}"
      done
    fi
    case "${attempt["command"]-}" in
    deploy)
      if [[ -v attempt["until"] ]]; then
        echo "${YELLOW}⏸${RESET}  ${attempt["timestamp"]} ${BOLD}[deploy ${attempt["target"]}]${RESET} paused at ${YELLOW}${attempt["until"]}${RESET}" >&2
        echo "   ${DIM}backup: ${attempt["backup_dir"]}, previous: ${attempt["previous_target"]}${RESET}" >&2
      else
        echo "${GREEN}✓${RESET}  ${attempt["timestamp"]} ${BOLD}[deploy ${attempt["target"]}]${RESET} ${GREEN}succeeded${RESET}" >&2
        echo "   ${DIM}backup: ${attempt["backup_dir"]}, previous: ${attempt["previous_target"]}${RESET}" >&2
      fi
      ;;
    restore)
      if [[ -v attempt["until"] ]]; then
        echo "${YELLOW}⏸${RESET}  ${attempt["timestamp"]} ${BOLD}[restore ${attempt["restore_timestamp"]}]${RESET} paused at ${YELLOW}${attempt["until"]}${RESET}" >&2
        echo "   ${DIM}restore: ${attempt["restore_dir"]}, backup: ${attempt["backup_dir"]}, target: ${attempt["target"]}, previous: ${attempt["previous_target"]}${RESET}" >&2
      else
        echo "${GREEN}✓${RESET}  ${attempt["timestamp"]} ${BOLD}[restore ${attempt["restore_timestamp"]}]${RESET} ${GREEN}succeeded${RESET}" >&2
        echo "   ${DIM}restore: ${attempt["restore_dir"]}, backup: ${attempt["backup_dir"]}, target: ${attempt["target"]}, previous: ${attempt["previous_target"]}${RESET}" >&2
      fi
      ;;
    *)
      print_error "Unsupported command '${attempt["command"]-}' in line ${line}"
      ;;
    esac
  done
  ;;

state)
  print_header "STATE"
  dotenv_target="$(fetch_target_from_dotenv)"
  if [[ -z "${dotenv_target}" ]]; then
    print_warning "Nothing is deployed according to ./.env"
  else
    print_success "Currently deployed target is '${CYAN}${dotenv_target}${RESET}' according to ./.env"
  fi
  echo >&2
  if [[ ${#attempt[@]} -eq 0 ]]; then
    print_info "No deployment attempt has been made yet."
  else
    if [[ ! -v attempt["until"] ]]; then
      #-----------------------------------------------
      # SUCCESS
      case "${attempt["command"]-}" in
      deploy)
        print_success "Deploy of '${CYAN}${attempt["target"]-}${RESET}' succeeded."
        ;;
      restore)
        print_success "Restore of '${CYAN}${attempt["restore_timestamp"]-}${RESET}' succeeded."
        ;;
      *)
        print_error "Unsupported command '${attempt["command"]-}'"
        exit 1
        ;;
      esac
    else
      #-----------------------------------------------
      # PAUSED
      case "${attempt["command"]-}" in
      deploy)
        print_warning "Paused deploy of '${CYAN}${attempt["target"]-}${RESET}' at step '${YELLOW}${attempt["until"]-}${RESET}'."
        print_info "Resume with \`${CYAN}./deploy.sh resume${RESET}\`"
        print_info "Rollback with \`${CYAN}./deploy.sh rollback${RESET}\`"
        ;;
      restore)
        print_warning "Paused restore of '${CYAN}${attempt["restore_timestamp"]-}${RESET}' at step '${YELLOW}${attempt["until"]-}${RESET}'."
        print_info "Resume with \`${CYAN}./deploy.sh resume${RESET}\`"
        print_info "Rollback with \`${CYAN}./deploy.sh rollback${RESET}\`"
        ;;
      *)
        print_error "Unsupported command '${attempt["command"]-}'"
        exit 1
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
    print_error "Failed during step '${STEP-unknown}'"
    echo "${DIM}Line '${line_number}'${RESET}" >&2
    echo "${DIM}Command '${bash_command}'${RESET}" >&2
    echo "${DIM}Exit code '${exit_code}'${RESET}" >&2

    append_or_overwrite_attempt

    pause() {
      echo >&2
      print_warning "Deployment paused. Fix the issue. Then either"
      print_info "resume with \`${CYAN}./deploy.sh resume${RESET}\` or"
      print_info "rollback with \`${CYAN}./deploy.sh rollback${RESET}\`."
      exit "${exit_code}"
    }

    rollback() {
      echo >&2
      print_info "Rolling back. Afterwards fix the issue. Then retry with \`${INVOCATION}\`"
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
      echo >&2
      while true; do
        read -rp "$(echo -e "${YELLOW}Do you want to [p]ause or [r]ollback?${RESET} ")" action
        case "${action,,}" in
        p | pause) pause ;;
        r | rollback) rollback ;;
        *) print_warning "Invalid choice. Please type 'p' for pause or 'r' for rollback." ;;
        esac
      done
      ;;
    esac
  }

  # Trap all exits (errors or manual cancels)
  trap 'cleanup_deploy_or_restore_or_resume "$?" "${LAST_LINENO}" "${BASH_COMMAND}"' EXIT

  #-----------------------------------------------
  # DO

  case "${COMMAND}" in
  deploy)
    print_header "DEPLOYING TARGET '${CYAN}${attempt["target"]-}${RESET}'"
    ;;
  restore)
    print_header "RESTORING DEPLOYMENT '${CYAN}${attempt["restore_timestamp"]-}${RESET}'"
    ;;
  resume)
    case "${attempt["command"]-}" in
    deploy)
      print_header "RESUMING DEPLOY of '${CYAN}${attempt["target"]-}${RESET}' at step '${YELLOW}${attempt["until"]-}${RESET})'"
      ;;
    restore)
      print_header "RESUMING RESTORE of '${CYAN}${attempt["restore_timestamp"]-}${RESET}' at step: '${YELLOW}${attempt["until"]-}${RESET})'"
      ;;
    *)
      print_error "Unsupported command '${attempt["command"]-}'"
      exit 1
      ;;
    esac
    ;;
  *)
    print_error "Unsupported command '${COMMAND}'"
    exit 1
    ;;
  esac

  case "${attempt["until"]-"begin-maintenance"}" in
  *) # run always
    STEP="begin-maintenance"
    attempt["until"]=${STEP}
    print_step "1" "9" "Beginning maintenance mode"
    run ./deploy.mk begin-maintenance || exit 1
    ;;&                 # continue with a proper match below
  begin-maintenance) ;& # fall through
  set-target)
    STEP="set-target"
    attempt["until"]=${STEP}
    print_step "2" "9" "Setting target in ./.env to '${CYAN}${attempt["target"]-}${RESET}'"
    attempt["previous_target"]="$(fetch_target_from_dotenv)"
    if [[ -z "${attempt[previous_target]}" ]]; then
      print_error "Previous target is missing in ./.env"
      exit 1
    fi
    if [[ -z "${attempt["target"]-}" ]]; then
      print_error "Target is unknown"
      exit 1
    fi
    run ./deploy.mk set-target TARGET="${attempt["target"]-}" || exit 1
    ;& # fall through
  backup)
    STEP="backup"
    attempt["until"]=${STEP}
    print_step "3" "9" "Backing up data into '${CYAN}${attempt["backup_dir"]-}${RESET}'"
    if [[ -z "${attempt["backup_dir"]-}" ]]; then
      print_error "Backup directory is unknown"
      exit 1
    fi
    run ./database.mk backup DIR="${attempt["backup_dir"]-}" || exit 1
    ;& # fall through
  switch)
    STEP="switch"
    attempt["until"]=${STEP}
    print_step "4" "9" "Fetching code from Git remote and switching to Git target '${CYAN}${attempt["target"]-}${RESET}'"
    if [[ -z "${attempt["target"]-}" ]]; then
      print_error "Target is unknown"
      exit 1
    fi
    run ./deploy.mk fetch-all || exit 1
    run ./deploy.mk switch TARGET="${attempt["target"]-}" || exit 1
    ;& # fall through
  dotenv)
    STEP="dotenv"
    attempt["until"]=${STEP}
    print_step "5" "9" "Checking dotenv file ./.env for compatibility with ./.env.production.yaml"
    run ./deploy.mk dotenv || exit 1
    ;& # fall through
  migrate-or-restore)
    STEP="migrate-or-restore"
    attempt["until"]=${STEP}
    case "${attempt["command"]-}" in
    deploy)
      print_step "6" "9" "Migrating PostgreSQL database"
      run ./database.mk migrate || exit 1
      ;;
    restore)
      print_step "6" "9" "Restoring data from '${CYAN}${attempt["restore_dir"]-}${RESET}'"
      if [[ -z "${attempt["restore_dir"]-}" ]]; then
        print_error "Restore directory is unknown"
        exit 1
      fi
      run ./database.mk restore DIR="${attempt["restore_dir"]-}" || exit 1
      ;;
    *)
      print_error "Unsupported command '${attempt["command"]-}'"
      exit 1
      ;;
    esac
    ;& # fall through
  services)
    STEP="services"
    attempt["until"]=${STEP}
    print_step "7" "9" "Recreating Docker Compose services"
    run ./deploy.mk services || exit 1
    ;& # fall through
  run-tests)
    STEP="run-tests"
    attempt["until"]=${STEP}
    print_step "8" "9" "Running tests"
    run ./deploy.mk run-tests || exit 1
    ;& # fall through
  end-maintenance)
    STEP="end-maintenance"
    attempt["until"]=${STEP}
    print_step "9" "9" "Ending maintenance mode"
    run ./deploy.mk end-maintenance || exit 1
    ;;
  esac

  unset "attempt[until]"
  append_or_overwrite_attempt

  echo >&2
  print_success "Completed successfully!"
  echo >&2

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
    print_error "Failed rolling back step '${STEP-unknown}'"
    echo "${DIM}Line '${line_number}'${RESET}" >&2
    echo "${DIM}Command '${bash_command}'${RESET}" >&2
    echo "${DIM}Exit code '${exit_code}'${RESET}" >&2
    print_warning "Rollback aborted. Fix the issue. Then retry with \`${INVOCATION}\`"
    exit "${exit_code}"
  }
  # Trap all exits (errors or manual cancels)
  trap 'cleanup_rollback "$?" "${LAST_LINENO}" "${BASH_COMMAND}"' EXIT

  #-----------------------------------------------
  # DO
  print_header "ROLLING BACK TO TARGET '${CYAN}${attempt[previous_target]}${RESET}'"

  rollback_steps() {
    STEP="begin-maintenance"
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0
    print_step "1" "9" "Beginning maintenance mode"
    run ./deploy.mk begin-maintenance || exit 1

    STEP="set-target"
    print_step "2" "9" "Setting target in ./.env to '${CYAN}${attempt[previous_target]-}${RESET}'"
    if [[ -z "${attempt[previous_target]-}" ]]; then
      print_error "Previous target is unknown"
      exit 1
    fi
    run ./deploy.mk set-target TARGET="${attempt[previous_target]-}" || exit 1
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    STEP="backup"
    # keep the backup for the step `migrate-or-restore`
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    STEP="switch"
    print_step "3" "9" "Fetching code from Git remote and switching to Git target '${CYAN}${attempt[previous_target]-}${RESET}'"
    if [[ -z "${attempt[previous_target]-}" ]]; then
      print_error "Previous target is unknown"
      exit 1
    fi
    run ./deploy.mk fetch-all || exit 1
    run ./deploy.mk switch TARGET="${attempt[previous_target]-}" || exit 1
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    STEP="dotenv"
    print_step "4" "9" "Checking dotenv file ./.env for compatibility with ./.env.production.yaml"
    run ./deploy.mk dotenv || exit 1
    [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    STEP="migrate-or-restore"
    print_step "5" "9" "Restoring data from '${CYAN}${attempt["backup_dir"]-}${RESET}'"
    if [[ -z "${attempt["backup_dir"]-}" ]]; then
      print_error "Backup directory is unknown"
      exit 1
    fi
    run ./database.mk restore DIR="${attempt["backup_dir"]-}" || exit 1
    # re-create services just to make sure instead of `[[ "${attempt["until"]}" == "${STEP}" ]] && return 0`

    STEP="services"
    print_step "6" "9" "Recreating Docker Compose services"
    run ./deploy.mk services || exit 1
    # run tests just to make sure instead of `[[ "${attempt["until"]}" == "${STEP}" ]] && return 0`

    STEP="run-tests"
    print_step "7" "9" "Running tests"
    run ./deploy.mk run-tests || exit 1
    # [[ "${attempt["until"]}" == "${STEP}" ]] && return 0

    return 0
  }
  rollback_steps

  STEP="backup"
  print_step "8" "9" "Removing data backup '${CYAN}${attempt["backup_dir"]-}${RESET}'"
  if [[ -n "${attempt["backup_dir"]-}" ]]; then
    run rm --recursive --force "${attempt["backup_dir"]-}" || exit 1
  else
    print_warning "Skipping because backup directory is unknown"
  fi

  STEP="end-maintenance"
  echo >&2
  print_step "9" "9" "Ending maintenance mode"
  run ./deploy.mk end-maintenance || exit 1

  remove_attempt

  echo >&2
  print_success "Rollback completed successfully!"

  ;;

esac
