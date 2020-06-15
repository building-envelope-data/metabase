#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mVerify that IKDB is empty                       \e[0m" >&2
./query.sh \
  http://ikdb.org:5000/graphql/ \
  verifyThatIkdbIsEmpty.graphql \
  "{}" \
  > /dev/null
echo -e "\e[33m===================================================\e[0m" >&2
