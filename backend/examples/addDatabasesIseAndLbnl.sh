#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mAdding databases ISE and LBNL           \e[0m" >&2
./query.sh \
  http://ikdb.org:5000/graphql/ \
  addDatabasesIseAndLbnl.graphql \
  "{ \
    \"iseInstitutionId\": \"$ISE_INSTITUTION_ID\", \
    \"lbnlInstitutionId\": \"$LBNL_INSTITUTION_ID\" \
  }" \
  > /dev/null
echo -e "\e[33m===================================================\e[0m" >&2
