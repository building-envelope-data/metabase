#!/bin/bash

echo -e "\e[33m===================================================\e[0m" >&2
echo -e "\e[33mVerify existence of institutions, databases, and components \e[0m" >&2
./query.sh \
  http://ikdb.org:5000/graphql/ \
  verifyExistenceOfInstitutionsDatabasesAndComponents.graphql \
  "{}" \
  > /dev/null
echo -e "\e[33m===================================================\e[0m" >&2
