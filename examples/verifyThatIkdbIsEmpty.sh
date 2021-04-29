#!/bin/bash

. ./functions.sh

begin_chapter "Verify that METABASE is empty"

query \
  $metabase_graphql_url \
  verifyThatMetabaseIsEmpty.graphql \
  "{}" \
  > /dev/null

end_chapter
