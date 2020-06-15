#!/bin/bash

./query.sh \
  http://ikdb.org:5000/graphql/ \
  getOpticalDataOfGlazingComponent.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\" \
  }" \
  | jq .
