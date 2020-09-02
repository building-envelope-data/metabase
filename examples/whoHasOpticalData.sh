#!/bin/bash

. ./functions.sh

begin_chapter "Who has optical data"

query \
  $metabase_graphql_url \
  whoHasOpticalData.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\" \
  }" \
  > /dev/null

end_chapter
