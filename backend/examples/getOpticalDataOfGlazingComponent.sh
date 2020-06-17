#!/bin/bash

. ./functions.sh

begin_chapter "Get optical data of glazing component"

query \
  http://ikdb.org:5000/graphql/ \
  getOpticalDataOfGlazingComponent.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\" \
  }" \
  true \
  > /dev/null

end_chapter
