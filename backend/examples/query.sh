#!/bin/bash

echo -e "\e[34m---------------------------------------------------\e[0m" >&2
echo -e "\e[34mAbout to send an HTTP POST request to GraphQL endpoint\e[0m" >&2
echo -e "\e[32mhttp://ikdb.org:5000/graphql/\e[0m \e[34mwith data ...\e[0m" >&2
echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2

read -p "Press any key to view data ..."                   >&2
echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2

nvim $2                                                    >&2
cat $2                                                     >&2

echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2
read -p "Press any key to send request ..."                >&2
echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2

json=$(
  curl \
    --verbose \
    --request POST \
    --header "Content-Type: application/json" \
    --data "{ \
      \"query\": \"$(cat $2 | sed 's/"/\\"/g' | tr '\n' ' ')\" \
      \"variables\": $3 \
    }" \
    $1 \
  )
    # --silent \
    # --show-error \

echo -e "\e[34m---------------------------------------------------\e[0m" >&2
echo -e "\e[34mReceived an HTTP response with JSON content ...\e[0m" >&2
echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2

read -p "Press any key to show response ..."                  >&2
echo -e "\e[32m- - - - - - - - - - - - - - - - - - - - - - - - - -\e[0m" >&2

# echo -e "$json" | jq .                                        >&2
echo "$json" | jq --color-output . | less --RAW-CONTROL-CHARS >&2
echo -e "$json" | jq .                                        >&2
echo -e "$json"
