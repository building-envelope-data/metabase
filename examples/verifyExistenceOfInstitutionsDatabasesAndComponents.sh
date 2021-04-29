#!/bin/bash

. ./functions.sh

begin_chapter "Verify existence of institutions, databases, and components"

query \
  $metabase_graphql_url \
  verifyExistenceOfInstitutionsDatabasesAndComponents.graphql \
  "{}" \
  true \
  > /dev/null

end_chapter
