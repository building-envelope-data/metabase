#!/bin/bash

. ./functions.sh

begin_chapter "Get optical data of glazing component"

query \
  $ikdb_graphql_url \
  getOpticalDataOfGlazingComponent.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\" \
  }" \
  true \
  > /dev/null

end_chapter
