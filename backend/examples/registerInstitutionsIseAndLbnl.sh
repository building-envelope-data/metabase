#!/bin/bash

read \
  ISE_INSTITUTION_ID \
  LBNL_INSTITUTION_ID \
  < <(echo $(
    ./query.sh \
      http://ikdb.org:5000/graphql/ \
      registerInstitutionsIseAndLbnl.graphql \
      "{}" \
      | jq .data[].institution.id \
      | tr --delete '"'
    )
  )

echo $ISE_INSTITUTION_ID
echo $LBNL_INSTITUTION_ID

export ISE_INSTITUTION_ID
export LBNL_INSTITUTION_ID
