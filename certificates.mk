#!/usr/bin/env -S make --file

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

jwt : ## Create JWT encryption and signing certificates if necessary
	docker compose pull \
		bootstrap
	docker compose build \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user) \
		bootstrap
	docker compose run \
		--rm \
		bootstrap \
		bash -ceux -o pipefail " \
			dotnet-script \
				/app/create-certificates.csx \
				-- \
				${JSON_WEB_TOKEN_ENCRYPTION_CERTIFICATE_PASSWORD} \
				${JSON_WEB_TOKEN_SIGNING_CERTIFICATE_PASSWORD} \
		"
.PHONY : jwt
