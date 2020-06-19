#!/bin/bash

. ./functions.sh

begin_chapter "Verify existence of institutions, databases, and components"

query \
  $ikdb_graphql_url \
  verifyExistenceOfInstitutionsDatabasesAndComponents.graphql \
  "{}" \
  > /dev/null

end_chapter
