#!/bin/bash

read \
  ISE_DATABASE_ID \
  LBNL_DATABASE_ID \
  < <(echo $(
    ./query.sh \
      http://ikdb.org:5000/graphql/ \
      addDatabasesIseAndLbnl.graphql \
      "{ \
        \"iseInstitutionId\": \"$ISE_INSTITUTION_ID\", \
        \"lbnlInstitutionId\": \"$LBNL_INSTITUTION_ID\" \
      }" \
      | jq .data[].database.id \
      | tr --delete '"'
    )
  )

echo $ISE_DATABASE_ID
echo $LBNL_DATABASE_ID
