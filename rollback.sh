#!/usr/bin/env bash
# [Bash Strict Mode](https://github.com/olivergondza/bash-strict-mode)
set -o errexit
set -o errtrace
set -o nounset
set -o pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

RED=$(tput setaf 1)
GREEN=$(tput setaf 2)
YELLOW=$(tput setaf 3)
RESET=$(tput sgr0)

TARGET=
BACKUP_DIR="${SCRIPT_DIR}/backup"
DRY_RUN=false
STEP="begin-maintenance"

usage() {
  cat <<EOF
Usage: $0 [options]

Options:
  --target <T>    The Git commit hash, tag, or branch to deploy (default \`cat ./.stored-target\`).
  --backup <DIR>  Path to store the data backup (default './backup').
  --step <STEP>   Start from: begin-maintenance, target, switch, dotenv, restore, services, run-tests, end-maintenance (default 'begin-maintenance').
  --dry-run       Print commands instead of running them.

Example: $0
         $0 --target v1.0.0 --backup ./backup --step restore
EOF
  exit 1
}

[[ $# -eq 0 ]] && usage

while [[ $# -gt 0 ]]; do
  case "$1" in
  --dry-run)
    DRY_RUN=true
    shift # skip to the next argument
    ;;
  --target)
    if [[ -z "$2" ]]; then
      echo "${RED}Error${RESET}: --target requires a value" >&2
      exit 1
    fi
    TARGET="$2"
    shift 2 # skip name and value
    ;;
  --backup)
    if [[ -z "$2" ]]; then
      echo "${RED}Error${RESET}: --backup requires a value" >&2
      exit 1
    fi
    BACKUP_DIR="$2"
    shift 2 # skip name and value
    ;;
  --step)
    if [[ -z "$2" ]]; then
      echo "${RED}Error${RESET}: --step requires a value" >&2
      exit 1
    fi
    STEP="$2"
    shift 2 # skip name and value
    ;;
  *)
    echo "${RED}Error${RESET}: Unknown argument $1" >&2
    exit 1
    ;;
  esac
done

if [[ -z "${TARGET}" ]]; then
  if [[ -f "${SCRIPT_DIR}/.stored-target" ]]; then
    TARGET="$(cat "${SCRIPT_DIR}/.stored-target")"
  else
    echo "${RED}Error${RESET}: No target was supplied and no target is stored in ./.stored-target." >&2
    exit 1
  fi
  usage
fi

if [[ -z "${BACKUP_DIR}" ]]; then
  echo "${RED}Error${RESET}: --backup is empty." >&2
  usage
fi

run() {
  if [ "$DRY_RUN" = true ]; then
    echo "${YELLOW}[DRY-RUN]${RESET} Would execute: $*" >&2
  else
    "$@"
  fi
}

cleanup() {
  local exit_code=$?
  [ $exit_code -eq 0 ] && exit 0 # Exit normally if no error

  echo
  echo "${RED}[!] Failed during step: ${STEP}${RESET}" >&2

  local command="./rollback.mk --target ${TARGET} --backup-dir ${BACKUP_DIR} --step ${STEP}"
  case "$STEP" in
  "begin-maintenance" | target | switch)
    echo "Everything is still up and running. Fix the rollback issue. Then continue with \`${command}\`. Ending maintenance mode for now." >&2
    ./deploy.mk end-maintenance
    ;;
  dotenv)
    echo "${RED}Critical${RESET}: System left in maintenance mode. Align ./.env with ./.env.production.sample. Then continue with \`${command}\`." >&2
    ;;
  restore | services | run-tests)
    echo "${RED}Critical${RESET}: System left in maintenance mode. Fix the rollback issue. Then continue with \`${command}\`." >&2
    ;;
  end-maintenance)
    echo "${RED}Critical${RESET}: System left in maintenance mode. Try to end it with \`${command}\`" >&2
    ;;
  esac
  exit $exit_code
}

# Trap all exits (errors or manual cancels)
trap cleanup EXIT

echo "Deploying target ${TARGET}" >&2

case "$STEP" in
*) # run always
  echo "${GREEN}Beginning maintenance mode${RESET}" >&2
  STEP="begin-maintenance"
  run ./deploy.mk begin-maintenance || exit 1
  ;;&                 # continue with another match below
begin-maintenance) ;& # fall through
target)
  echo "${GREEN}Setting target in ./.env to ${TARGET}${RESET}" >&2
  STEP="target"
  run ./deploy.mk set-target TARGET="${TARGET}" || exit 2
  ;& # fall through
switch)
  echo "${GREEN}Fetching code from Git remote and switching to Git target ${TARGET}${RESET}" >&2
  STEP="switch"
  run ./deploy.mk fetch-all || exit 3
  run ./deploy.mk switch TARGET="${TARGET}" || exit 4
  ;& # fall through
dotenv)
  echo "${GREEN}Checking dotenv file ./.env for compatibility with ./.env.production.yaml${RESET}" >&2
  STEP="dotenv"
  run ./deploy.mk dotenv || exit 5
  ;& # fall through
restore)
  echo "${GREEN}Restoring data from ${BACKUP_DIR}${RESET}" >&2
  STEP="backup"
  run ./deploy.mk restore DIR="${BACKUP_DIR}" || exit 6
  ;& # fall through
services)
  echo "${GREEN}Recreating Docker Compose services${RESET}" >&2
  STEP="services"
  run ./deploy.mk services || exit 7
  ;& # fall through
run-tests)
  echo "${GREEN}Running tests${RESET}" >&2
  STEP="tests"
  run ./deploy.mk run-tests || exit 8
  ;& # fall through
end-maintenance)
  echo "${GREEN}Ending maintenance mode${RESET}" >&2
  STEP="dotenv"
  run ./deploy.mk end-maintenance || exit 9
  ;;
esac

echo "Successfully deployed ${TARGET}." >&2
