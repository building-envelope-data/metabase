#!/usr/bin/env bash
# [Bash Strict Mode](https://github.com/olivergondza/bash-strict-mode)
set -o errexit
set -o errtrace
set -o nounset
set -o pipefail

source ./utils.sh

TARGET=
RESUME=true # needed for `utils.sh#write_attempt`
ON_ERROR=pause
DRY_RUN=false
STEP="begin-maintenance"
BACKUP_DIR=

usage() {
  local script_name
  script_name=$(basename "$0")
  cat <<EOF

${script_name} - Rollback deployment attempt

USAGE:
  ${script_name} [options]

OPTIONS:
  -d, --dry-run             Print commands instead of running them.
  -h, --help                Display this help message.

EXAMPLES:
  ${script_name}
  ${script_name} --dry-run
EOF
  exit 1
}

while [[ $# -gt 0 ]]; do
  case "$1" in
  -d | --dry-run)
    DRY_RUN=true
    shift # skip to the next option
    ;;
  -h | --help)
    usage
    ;;
  *)
    echo "${RED}Error${RESET}: Unknown option $1" >&2
    usage
    ;;
  esac
done

read_attempt
TARGET="${attempt["previous-target"]}"
BACKUP_DIR="${attempt["backup-dir"]:${BACKUP_DIR}}"
if [[ -z "${TARGET}" ]]; then
  echo "[${RED}Error${RESET}] Cannot rollback deployment attempt. It does not have a previous Git target." >&2
  exit 1
fi
if [[ -z "${BACKUP_DIR}" ]]; then
  echo "[${RED}Error${RESET}] Cannot rollback deployment attempt. It does not have a backup directory." >&2
  exit 1
fi

cleanup() {
  local exit_code=$?
  [ $exit_code -eq 0 ] && exit 0 # Exit normally if no error

  echo >&2
  echo "${RED}[Error]${RESET} Failed during step: ${STEP}" >&2
  write_attempt

  pause() {
    echo "Pausing rollback attempt. Fix the rollback issue. Then resume with \`./rollback.sh\` (or \`./deploy.mk rollback\`)"
    exit ${exit_code}
  }

  case "${ON_ERROR}" in
  pause | *)
    pause
    ;;
  esac
}

# Trap all exits (errors or manual cancels)
trap cleanup EXIT

echo "Rolling back to target ${TARGET}" >&2

case "$STEP" in
*) # run always
  STEP="begin-maintenance"
  echo "${GREEN}[STEP]${RESET} Beginning maintenance mode" >&2
  run ./deploy.mk begin-maintenance || exit 1
  ;;&                 # continue with a proper match below
begin-maintenance) ;& # fall through
target)
  STEP="target"
  if [[ -v attempt[${STEP}] ]] && [[ "${attempt[${STEP}]}" != "${ROLLED_BACK}" ]]; then
    echo "${GREEN}[STEP]${RESET} Setting target in ./.env to ${TARGET}" >&2
    run ./deploy.mk set-target TARGET="${TARGET}" || exit 1
    attempt["${STEP}"]="${ROLLED_BACK}"
  fi
  ;& # fall through
switch)
  STEP="switch"
  if [[ -v attempt[${STEP}] ]] && [[ "${attempt[${STEP}]}" != "${ROLLED_BACK}" ]]; then
    echo "${GREEN}[STEP]${RESET} Fetching code from Git remote and switching to Git target ${TARGET}" >&2
    run ./deploy.mk fetch-all || exit 1
    run ./deploy.mk switch TARGET="${TARGET}" || exit 1
    attempt["${STEP}"]="${ROLLED_BACK}"
  fi
  ;& # fall through
dotenv)
  STEP="dotenv"
  if [[ "${attempt[${STEP}]:-}" != "${ROLLED_BACK}" ]]; then
    echo "${GREEN}[STEP]${RESET} Checking dotenv file ./.env for compatibility with ./.env.production.yaml" >&2
    run ./deploy.mk dotenv || exit 1
    attempt["${STEP}"]="${ROLLED_BACK}"
  fi
  ;& # fall through
backup)
  STEP="backup"
  if [[ -v attempt[${STEP}] ]] && [[ "${attempt[${STEP}]}" == "${SUCCEEDED}" ]]; then
    echo "${GREEN}[STEP]${RESET} Restoring data from ${BACKUP_DIR}" >&2
    run ./deploy.mk restore DIR="${BACKUP_DIR}" || exit 1
    attempt[${STEP}]="${ROLLED_BACK}"
  fi
  ;& # fall through
services)
  STEP="services"
  if [[ "${attempt[${STEP}]:-}" != "${ROLLED_BACK}" ]]; then
    echo "${GREEN}[STEP]${RESET} Recreating Docker Compose services" >&2
    run ./deploy.mk services || exit 1
    attempt["${STEP}"]="${ROLLED_BACK}"
  fi
  ;& # fall through
run-tests)
  STEP="run-tests"
  if [[ "${attempt[${STEP}]:-}" != "${ROLLED_BACK}" ]]; then
    echo "${GREEN}[STEP]${RESET} Running tests" >&2
    run ./deploy.mk run-tests || exit 1
    attempt["${STEP}"]="${ROLLED_BACK}"
  fi
  ;& # fall through
end-maintenance)
  STEP="end-maintenance"
  echo "${GREEN}[STEP]${RESET} Ending maintenance mode" >&2
  run ./deploy.mk end-maintenance || exit 1
  ;;
esac

write_attempt

echo "Successfully rolled back to ${TARGET}." >&2
