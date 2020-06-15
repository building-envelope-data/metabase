#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mAdd optical data to LBNL database    \e[0m" >&2
json=$(
  ./query.sh \
    http://lbnl.gov:5020/graphql/ \
    addOpticalDataToLbnlDatabase.graphql \
    "{ \
      \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\" \
    }" \
)
echo -e "\e[34m---------------------------------------------------\e[0m" >&2
echo -e "\e[34mStored LBNL glazing optical data timestamp in variable ... \e[0m" >&2
echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2
read \
  LBNL_GLAZING_OPTICAL_DATA_TIMESTAMP \
  < <(echo $(
      echo "$json" \
      | jq .data[].opticalData.timestamp \
      | tr --delete '"'
    )
  )
echo -e "LBNL glazing optical data timestamp: \e[32m$LBNL_GLAZING_OPTICAL_DATA_TIMESTAMP\e[0m" >&2
echo -e "\e[33m===================================================\e[0m" >&2
