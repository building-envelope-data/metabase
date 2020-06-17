#!/bin/bash

. ./functions.sh

begin_chapter "Verify existence of institutions, databases, and components"

query \
  http://ikdb.org:5000/graphql/ \
  verifyExistenceOfInstitutionsDatabasesAndComponents.graphql \
  "{}" \
  true \
  > /dev/null

end_chapter
