#!/bin/bash

./query.sh \
  http://testlab-solar-facades.de:5010/graphql/ \
  addOpticalDataToLbnlDatabase.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\" \
  }"
