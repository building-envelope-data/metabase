#!/bin/bash

. ./functions.sh

begin_chapter "Add optical data to ISE database"

query \
  $ise_graphql_url \
  addOpticalDataToIseDatabase.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\" \
  }" \
  > /dev/null

end_chapter
