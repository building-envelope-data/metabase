#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mGenerating component identifiers        \e[0m" >&2
json=$(
  ./query.sh \
    http://ikdb.org:5000/graphql/ \
    generateComponentIds.graphql \
    "{}" \
)
echo -e "\e[34m---------------------------------------------------\e[0m" >&2
echo -e "\e[34mStored component identifiers in variables ...     \e[0m" >&2
echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2
read \
  GLAZING_COMPONENT_ID \
  SHADING_COMPONENT_ID \
  < <(echo $(
      echo "$json" \
      | jq .data[].component.id \
      | tr --delete '"'
    )
  )
echo -e "Glazing component identifier: \e[32m$GLAZING_COMPONENT_ID\e[0m" >&2
echo -e "Shading component identifier: \e[32m$SHADING_COMPONENT_ID\e[0m" >&2
echo -e "\e[33m===================================================\e[0m" >&2

read -p "Press any key to continue ..."

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mAdding component manufacturers          \e[0m" >&2
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
echo -e "\e[33m===================================================\e[0m" >&2
