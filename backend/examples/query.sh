#!/bin/bash

curl \
  --silent \
  --request POST \
  --header "Content-Type: application/json" \
  --data "{ \
    \"query\": \"$(cat $2 | sed 's/"/\\"/g' | tr '\n' ' ')\" \
    \"variables\": $3 \
  }" \
  $1
