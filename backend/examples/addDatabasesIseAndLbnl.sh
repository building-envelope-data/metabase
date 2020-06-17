#!/bin/bash

. ./functions.sh

begin_chapter "Add databases ISE and LBNL"

query \
  http://ikdb.org:5000/graphql/ \
  addDatabasesIseAndLbnl.graphql \
  "{ \
    \"iseInstitutionId\": \"$ISE_INSTITUTION_ID\", \
    \"lbnlInstitutionId\": \"$LBNL_INSTITUTION_ID\" \
  }" \
  > /dev/null

end_chapter
