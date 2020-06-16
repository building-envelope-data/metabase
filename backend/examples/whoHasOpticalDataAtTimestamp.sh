#!/bin/bash

. ./functions.sh

begin_chapter "Who has optical data at timestamp"

query \
  http://ikdb.org:5000/graphql/ \
  whoHasOpticalDataAtTimestamp.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\", \
    \"lbnlGlazingOpticalDataTimestamp\": \"$LBNL_GLAZING_OPTICAL_DATA_TIMESTAMP\" \
  }" \
  > /dev/null

end_chapter
