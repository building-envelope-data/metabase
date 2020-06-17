begin_chapter "Add component manufacturers"

query \
  http://ikdb.org:5000/graphql/ \
  addComponentManufacturers.graphql \
  "{ \
    \"glazingComponentId\": \"$GLAZING_COMPONENT_ID\", \
    \"shadingComponentId\": \"$SHADING_COMPONENT_ID\", \
    \"iseInstitutionId\": \"$ISE_INSTITUTION_ID\", \
    \"lbnlInstitutionId\": \"$LBNL_INSTITUTION_ID\" \
  }" \
  > /dev/null

end_chapter