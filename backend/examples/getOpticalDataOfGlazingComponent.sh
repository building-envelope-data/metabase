#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mGetting optical data of glazing component\e[0m" >&2
./query.sh \
  http://ikdb.org:5000/graphql/ \
  getOpticalDataOfGlazingComponent.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\" \
  }" \
  > /dev/null
echo -e "\e[33m===================================================\e[0m" >&2
