#!/bin/bash

read \
  GLAZING_COMPONENT_ID \
  SHADING_COMPONENT_ID \
  < <(echo $(
    ./query.sh \
      http://ikdb.org:5000/graphql/ \
      generateComponentIds.graphql \
      "{}" \
      | jq .data[].component.id \
      | tr --delete '"'
    )
  )

echo $GLAZING_COMPONENT_ID
echo $SHADING_COMPONENT_ID

./query.sh \
  http://ikdb.org:5000/graphql/ \
  addComponentManufacturers.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\", \
    \"iseInstitutionId\": \"$ISE_INSTITUTION_ID\", \
    \"lbnlInstitutionId\": \"$LBNL_INSTITUTION_ID\" \
  }" \
  > /dev/null

export GLAZING_COMPONENT_ID
export SHADING_COMPONENT_ID
