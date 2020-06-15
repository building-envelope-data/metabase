#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mAdding optical data to ISE databasee    \e[0m" >&2
./query.sh \
  http://testlab-solar-facades.de:5010/graphql/ \
  addOpticalDataToIseDatabase.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\" \
  }" \
  > /dev/null
echo -e "\e[33m===================================================\e[0m" >&2
