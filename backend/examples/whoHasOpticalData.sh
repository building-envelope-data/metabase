#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mWho has optical data                               \e[0m" >&2
./query.sh \
  http://ikdb.org:5000/graphql/ \
  whoHasOpticalData.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\" \
  }" \
  > /dev/null
echo -e "\e[33m===================================================\e[0m" >&2
