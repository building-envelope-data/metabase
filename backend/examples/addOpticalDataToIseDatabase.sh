#!/bin/bash

. ./functions.sh

begin_chapter "Add optical data to ISE database"

query \
  http://testlab-solar-facades.de:5010/graphql/ \
  addOpticalDataToIseDatabase.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\" \
  }" \
  > /dev/null

end_chapter
