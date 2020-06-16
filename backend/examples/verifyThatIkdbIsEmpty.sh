#!/bin/bash

. ./functions.sh

begin_chapter "Verify that IKDB is empty"

query \
  http://ikdb.org:5000/graphql/ \
  verifyThatIkdbIsEmpty.graphql \
  "{}" \
  > /dev/null

end_chapter
