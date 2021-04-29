chapter_color="\e[33m"
section_color="\e[34m"
paragraph_color="\e[32m"
reset_color="\e[0m"
green_color="\e[32m"
chapter_separator="==================================================="
section_separator="---------------------------------------------------"
paragraph_separator="- - - - - - - - - - - - - - - - - - - - - - - - - -"

metabase_graphql_url="https://local.buildingenvelopedata.org:4041/graphql/"
ise_graphql_url="https://local.testlab-solar-facades.de:4051/graphql/"
lbnl_graphql_url="https://local.lbnl.gov:4061/graphql/"

function echo_error() {
  local message="$1"
  echo -e "$message" >&2
}

function begin_x() {
  local separator="$1"
  local color="$2"
  local title="$3"
  local subtitle="$4"
  if [ "$separator" != "" ]; then
    echo_error "${color}${separator}${reset_color}"
  fi
  if [ "$title" != "" ]; then
    echo_error "${color}${title}${reset_color}"
  fi
  if [ "$subtitle" != "" ]; then
    echo_error "${color}${subtitle}${reset_color}"
  fi
}

function end_x() {
  local separator="$1"
  local color="$2"
  echo_error "${color}${separator}${reset_color}"
}

function begin_chapter() {
  begin_x "$chapter_separator" "$chapter_color" "$1" "$2"
}

function end_chapter() {
  end_x "$chapter_separator" "$chapter_color"
}

function begin_section() {
  begin_x "$section_separator" "$section_color" "$1" "$2"
}

function end_section() {
  end_x "$section_separator" "$section_color"
}

function begin_paragraph() {
  begin_x "$paragraph_separator" "$paragraph_color" "" ""
}

function end_paragraph() {
  end_x "$paragraph_separator" "$paragraph_color"
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

  begin_section \
    "About to send an HTTP POST request to GraphQL endpoint" \
    "${green_color}${graphql_endpoint_url}${reset_color}${section_color} with data ...${reset_color}"
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
    # In NeoVIM normal mode use `za` to toggle fold, `zo` to open fold, and
    # `zc` to close fold
    nvim -R \
      -u <(printf " \
        set foldenable \n \
        set foldmethod=syntax \n \
        set foldlevel=6 \
      ") \
      $json_file_path \
      >&2
  fi
  # echo "$json" | jq --color-output . | less --RAW-CONTROL-CHARS >&2
  jq . $json_file_path >&2
  echo $json_file_path
}
