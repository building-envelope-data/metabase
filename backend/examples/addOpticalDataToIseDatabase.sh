#!/bin/bash

./query.sh \
  http://testlab-solar-facades.de:5010/graphql/ \
  addOpticalDataToIseDatabase.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\" \
  }"
