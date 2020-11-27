#!/bin/bash

. ./functions.sh

begin_chapter "Add databases ISE and LBNL"

query \
  $metabase_graphql_url \
  addDatabasesIseAndLbnl.graphql \
  "{ \
    \"iseDatabaseLocator\": \"$ise_graphql_url\", \
    \"lbnlDatabaseLocator\": \"$lbnl_graphql_url\", \
    \"iseInstitutionId\": \"$ISE_INSTITUTION_ID\", \
    \"lbnlInstitutionId\": \"$LBNL_INSTITUTION_ID\" \
  }" \
  > /dev/null

end_chapter
