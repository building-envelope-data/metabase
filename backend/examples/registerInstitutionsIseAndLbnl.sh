#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mRegistering institutions ISE and LBNL   \e[0m" >&2
json=$(
  ./query.sh \
    http://ikdb.org:5000/graphql/ \
    registerInstitutionsIseAndLbnl.graphql \
    "{}" \
)
echo -e "\e[34m---------------------------------------------------\e[0m" >&2
echo -e "\e[34mStored institution identifiers in variables ...   \e[0m" >&2
echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2
read \
  ISE_INSTITUTION_ID \
  LBNL_INSTITUTION_ID \
  < <(echo $(
      echo "$json" \
      | jq .data[].institution.id \
      | tr --delete '"'
    )
  )
echo -e "ISE identifier: \e[32m$ISE_INSTITUTION_ID\e[0m                        " >&2
echo -e "LBNL identifier: \e[32m$LBNL_INSTITUTION_ID\e[0m                      " >&2
echo -e "\e[33m===================================================\e[0m" >&2
