#!/bin/bash

. ./functions.sh

begin_chapter "Generate component identifiers"

json_file_path=$(
  query \
    $ikdb_graphql_url \
    generateComponentIds.graphql \
    "{}" \
)

begin_section "Stored component identifiers in variables ..."
begin_paragraph

read \
  GLAZING_COMPONENT_ID \
  SHADING_COMPONENT_ID \
  < <(echo $(
      cat $json_file_path \
      | jq .data[].component.id \
      | tr -d '"'
    )
  )
echo_error "Glazing component identifier: \e[32m$GLAZING_COMPONENT_ID\e[0m"
echo_error "Shading component identifier: \e[32m$SHADING_COMPONENT_ID\e[0m"

end_chapter
