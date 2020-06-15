#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mWho has optical data at timestamp                  \e[0m" >&2
./query.sh \
  http://ikdb.org:5000/graphql/ \
  whoHasOpticalDataAtTimestamp.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\", \
    \"lbnlGlazingOpticalDataTimestamp\": \"$LBNL_GLAZING_OPTICAL_DATA_TIMESTAMP\" \
  }" \
  > /dev/null
echo -e "\e[33m===================================================\e[0m" >&2
