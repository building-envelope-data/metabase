#!/bin/bash

begin_chapter "Register institutions ISE and LBNL"

json_file_path=$(
  query \
    $metabase_graphql_url \
    registerInstitutionsIseAndLbnl.graphql \
    "{}" \
)

begin_section "Stored institution identifiers in variables ..."
begin_paragraph

read \
  ISE_INSTITUTION_ID \
  LBNL_INSTITUTION_ID \
  < <(echo $(
      cat $json_file_path \
      | jq .data[].institution.id \
      | tr -d '"'
    )
  )
echo_error "ISE identifier: \e[32m$ISE_INSTITUTION_ID\e[0m"
echo_error "LBNL identifier: \e[32m$LBNL_INSTITUTION_ID\e[0m"

end_chapter
