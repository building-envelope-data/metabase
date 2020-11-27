#!/bin/bash

. ./functions.sh

begin_chapter \
  "Who has optical data at timestamp ${green_color}${LBNL_GLAZING_OPTICAL_DATA_TIMESTAMP}${reset_color}${chapter_color}, that is," \
  "after optical data was added to the LBNL database for the glazing component\nand before any data was added to the ISE database"

query \
  $metabase_graphql_url \
  whoHasOpticalDataAtTimestamp.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\", \
    \"lbnlGlazingOpticalDataTimestamp\": \"$LBNL_GLAZING_OPTICAL_DATA_TIMESTAMP\" \
  }" \
  > /dev/null

end_chapter
