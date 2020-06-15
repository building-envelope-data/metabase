#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mAdding optical data to LBNL database    \e[0m" >&2
./query.sh \
  http://lbnl.gov:5020/graphql/ \
  addOpticalDataToLbnlDatabase.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\" \
  }" \
  > /dev/null
echo -e "\e[33m===================================================\e[0m" >&2
