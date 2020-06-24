#!/bin/bash

. ./functions.sh

begin_chapter "Verify that IKDB is empty"

query \
  $ikdb_graphql_url \
  verifyThatIkdbIsEmpty.graphql \
  "{}" \
  > /dev/null

end_chapter
