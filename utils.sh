RED=$(tput setaf 1)
GREEN=$(tput setaf 2)
YELLOW=$(tput setaf 3)
RESET=$(tput sgr0)

SUCCEEDED="succeeded"
FAILED="failed"
ROLLED_BACK="rolled-back"
HISTORY_PATH=./.deploy-histor

touch ${HISTORY_PATH}

run() {
  if "$DRY_RUN"; then
    echo "${YELLOW}[DRY-RUN]${RESET} Would execute: $*" >&2
  else
    "$@"
  fi
}

declare -A attempt # associative array

read_attempt() {
  echo "Reading attempt from history ${HISTORY_PATH}" >&2
  IFS=',' read -ra pairs < <(tail --lines=1 <(grep . "${HISTORY_PATH}"))
  for pair in "${pairs[@]}"; do
    key="${pair%%=*}"
    value="${pair#*=}"
    attempt["${key}"]="${value}"
  done
}

write_attempt() {
  echo "Writing attempt to history ${HISTORY_PATH}" >&2
  entries=()
  for key in "${!attempt[@]}"; do
    entries+=("$key=${attempt[$key]}")
  done
  line="$(
    IFS=,
    echo "${entries[*]}"
  )"
  if ${RESUME}; then
    run sed --in-place "\$c ${line}" "${HISTORY_PATH}"
  else
    run echo "${line}" >>"${HISTORY_PATH}"
  fi
}
