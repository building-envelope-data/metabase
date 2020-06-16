chapter_separator="\e[33m===================================================\e[0m"
section_separator="\e[34m---------------------------------------------------\e[0m"
paragraph_separator="\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m"

function echo_error() {
  local message="$1"
  echo -e "$message" >&2
}

function begin_chapter() {
  local title="$1"
  echo_error "$chapter_separator"
  echo_error "\e[33m$title\e[0m"
}

function end_chapter() {
  echo_error "$chapter_separator"
}

function begin_section() {
  local title="$1"
  echo_error "$section_separator"
  echo_error "\e[34m$title\e[0m"
}

function begin_paragraph() {
  echo_error "$paragraph_separator"
}

function end_paragraph() {
  echo_error "$paragraph_separator"
}

function press_any_key_to() {
  local action="$1"
  local key=
  begin_paragraph
  read \
    -s \
    -n 1 \
    -p "Press any key to ${action} ..." \
    key \
    >&2
  echo_error ""
  end_paragraph
  echo $key
}

function query() {
  local graphql_endpoint_url="$1"
  local graphql_file_path="$2"
  local graphql_variables_json="$3"
  local has_big_response="${4:false}"
  local json_file_path=$(echo $graphql_file_path | sed -e "s/\.graphql$/\.json/")

  begin_section "About to send an HTTP POST request to GraphQL endpoint"
  echo_error "\e[32mhttp://ikdb.org:5000/graphql/\e[0m \e[34mwith data ...\e[0m"
  if [ "$(press_any_key_to "view data")" = n ]; then
    nvim $graphql_file_path >&2
  fi
  cat $graphql_file_path >&2

  press_any_key_to "send request" > /dev/null
    # --silent \
    # --show-error \
  curl \
    --verbose \
    --request POST \
    --header "Content-Type: application/json" \
    --data "{ \
      \"query\": \"$(cat $graphql_file_path | sed 's/"/\\"/g' | tr '\n' ' ')\" \
      \"variables\": $graphql_variables_json \
    }" \
    $graphql_endpoint_url \
    | jq . \
    > $json_file_path

  begin_section "Received an HTTP response with JSON content ..."
  press_any_key_to "show content" > /dev/null
  if [ "$has_big_response" = true ]; then
    nvim $json_file_path >&2
  fi
  # echo "$json" | jq --color-output . | less --RAW-CONTROL-CHARS >&2
  jq . $json_file_path >&2
  echo $json_file_path
}
