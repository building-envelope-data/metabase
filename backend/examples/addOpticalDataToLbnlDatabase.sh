#!/bin/bash

./query.sh \
  http://lbnl.gov:5020/graphql/ \
  addOpticalDataToLbnlDatabase.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\" \
  }"
