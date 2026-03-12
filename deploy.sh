#!/usr/bin/env bash
# [Bash Strict Mode](https://github.com/olivergondza/bash-strict-mode)
set -o errexit
set -o errtrace
set -o nounset
set -o pipefail

source ./utils.sh

TARGET=
RESUME=false
ON_ERROR=pause
DRY_RUN=false
STEP="begin-maintenance"
BACKUP_DIR=/app/data/backups/$(date +"%Y-%m-%d_%H_%M_%S")

usage() {
  local script_name
  script_name=$(basename "$0")
  cat <<EOF

${script_name} - Deploy a target or resume a paused attempt

USAGE:
  ${script_name} --target <GIT_TARGET> [options]
  ${script_name} --resume [options]

REQUIRED (Choose one):
  -t, --target <GIT_TARGET> Git commit hash, tag, or branch to deploy.
  -r, --resume              Resume a paused deployment attempt.

OPTIONS:
  -e, --on-error <ACTION>   When an error occurs then: pause, restore (previous deployment), or ask (for user input). (default: pause)
  -d, --dry-run             Print commands instead of running them.
  -h, --help                Display this help message.

EXAMPLES:
  ${script_name} --target v1.0.0 --on-error pause
  ${script_name} --resume
EOF
  exit 1
}

[[ $# -eq 0 ]] && usage

while [[ $# -gt 0 ]]; do
  case "$1" in
  -t | --target)
    if [[ -z "$2" ]]; then
      echo "[${RED}Error${RESET}] --target requires a value" >&2
      exit 1
    fi
    TARGET="$2"
    shift 2 # skip name and value
    ;;
  -r | --resume)
    RESUME=true
    shift # skip to the next option
    ;;
  -e | --on-error)
    if [[ -z "$2" ]]; then
      echo "[${RED}Error${RESET}] --on-error requires a value" >&2
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
    echo "[${RED}Error${RESET}] Unknown option $1" >&2
    usage
    ;;
  esac
done

if ([[ -z "${TARGET}" ]] && ! ${RESUME}) || ([[ ! -z "${TARGET}" ]] && ${RESUME}); then
  echo "[${RED}Error${RESET}] use either --target or --resume." >&2
  usage
fi

case "${ON_ERROR}" in
pause | restore | ask) ;;
*)
  echo "[${RED}Error${RESET}] --on-error is neither 'pause' nor 'restore' nor 'ask' but '${ON_ERROR}'." >&2
  usage
  ;;
esac

if ${RESUME}; then
  read_attempt
  TARGET="${attempt["next-target"]:-}"
  BACKUP_DIR="${attempt["backup-dir"]:${BACKUP_DIR}}"
  if [[ -z "${TARGET}" ]]; then
    echo "[${RED}Error${RESET}] Cannot resume deployment attempt. The paused attempt does not have a Git target. Make a fresh deployment attempt with \`./deploy.sh --target ...\` (or \`./deploy.mk do TARGET=...\`)" >&2
    exit 1
  fi
else
  attempt["datetime"]="$(date --iso-8601=seconds)"
  attempt["next-target"]="${TARGET}"
  attempt["backup-dir"]="${BACKUP_DIR}"
fi

cleanup() {
  local exit_code=$?
  [ $exit_code -eq 0 ] && exit 0 # Exit normally if no error

  echo >&2
  echo "${RED}[Error]${RESET} Failed during step: ${STEP}" >&2

  attempt[${STEP}]=${FAILED}
  write_attempt

  pause() {
    echo "Pausing deployment attempt. Fix the deployment issue. Then resume with \`./deploy.sh --resume\` or rollback wtih \`./rollback.sh\` (or \`./deploy.mk resume\` or \`./deploy.mk rollback\`)"
    exit ${exit_code}
  }

  restore() {
    echo "Restoring previous deployment. Fix the deployment issue. Then retry with \`./deploy.sh --target ${TARGET} --on-error ${ON_ERROR}\` (or \`./deploy.mk do TARGET=${TARGET}\`)"
    run ./restore.sh
    exit ${exit_code}
  }

  case "${ON_ERROR}" in
  pause)
    pause
    ;;
  restore)
    restore
    ;;
  ask | *)
    while true; do
      read -rp "Do you want to [p]ause or [r]estore? " action
      case "${action,,}" in
      p | pause) pause ;;
      r | restore) restore ;;
      *) echo "Invalid choice. Please type 'p' or 'r' or 'pause' or 'restore'." ;;
      esac
    done
    ;;
  esac
}

# Trap all exits (errors or manual cancels)
trap cleanup EXIT

echo "Deploying target ${TARGET}" >&2

case "$STEP" in
*) # run always
  STEP="begin-maintenance"
  echo "${GREEN}[STEP]${RESET} Beginning maintenance mode" >&2
  run ./deploy.mk begin-maintenance || exit 1
  ;;&                 # continue with a proper match below
begin-maintenance) ;& # fall through
target)
  STEP="target"
  if [[ "${attempt[${STEP}]:-}" != "${SUCCEEDED}" ]]; then
    echo "${GREEN}[STEP]${RESET} Setting target in ./.env to ${TARGET}" >&2
    attempt["previous-target"]="$(grep --only-matching --perl-regexp '(?<=TARGET=).*' ./.env)"
    run ./deploy.mk set-target TARGET="${TARGET}" || exit 1
    attempt["${STEP}"]="${SUCCEEDED}"
  fi
  ;& # fall through
backup)
  STEP="backup"
  if [[ "${attempt[${STEP}]:-}" != "${SUCCEEDED}" ]]; then
    echo "${GREEN}[STEP]${RESET} Backing up data into ${BACKUP_DIR}" >&2
    run ./deploy.mk backup DIR="${BACKUP_DIR}" || exit 1
    attempt["${STEP}"]="${SUCCEEDED}"
  fi
  ;& # fall through
switch)
  STEP="switch"
  if [[ "${attempt[${STEP}]:-}" != "${SUCCEEDED}" ]]; then
    echo "${GREEN}[STEP]${RESET} Fetching code from Git remote and switching to Git target ${TARGET}" >&2
    run ./deploy.mk fetch-all || exit 1
    run ./deploy.mk switch TARGET="${TARGET}" || exit 1
    attempt["${STEP}"]="${SUCCEEDED}"
  fi
  ;& # fall through
dotenv)
  STEP="dotenv"
  if [[ "${attempt[${STEP}]:-}" != "${SUCCEEDED}" ]]; then
    echo "${GREEN}[STEP]${RESET} Checking dotenv file ./.env for compatibility with ./.env.production.yaml" >&2
    run ./deploy.mk dotenv || exit 1
    attempt["${STEP}"]="${SUCCEEDED}"
  fi
  ;& # fall through
migrate)
  STEP="migrate"
  if [[ "${attempt[${STEP}]:-}" != "${SUCCEEDED}" ]]; then
    echo "${GREEN}[STEP]${RESET} Migrating PostgreSQL database" >&2
    run ./deploy.mk migrate || exit 1
    attempt["${STEP}"]="${SUCCEEDED}"
  fi
  ;& # fall through
services)
  STEP="services"
  if [[ "${attempt[${STEP}]:-}" != "${SUCCEEDED}" ]]; then
    echo "${GREEN}[STEP]${RESET} Recreating Docker Compose services" >&2
    run ./deploy.mk services || exit 1
    attempt["${STEP}"]="${SUCCEEDED}"
  fi
  ;& # fall through
run-tests)
  STEP="run-tests"
  if [[ "${attempt[${STEP}]:-}" != "${SUCCEEDED}" ]]; then
    echo "${GREEN}[STEP]${RESET} Running tests" >&2
    run ./deploy.mk run-tests || exit 1
    attempt["${STEP}"]="${SUCCEEDED}"
  fi
  ;& # fall through
end-maintenance)
  STEP="end-maintenance"
  echo "${GREEN}[STEP]${RESET} Ending maintenance mode" >&2
  run ./deploy.mk end-maintenance || exit 1
  ;;
esac

write_attempt

echo "Successfully deployed ${TARGET}." >&2
