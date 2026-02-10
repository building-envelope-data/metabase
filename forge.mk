#!/usr/bin/env -S make --file
SELF := $(lastword $(MAKEFILE_LIST))

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

NAME = metabase
TARGET = $(shell git rev-parse --verify HEAD)

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

target : ## Print value of variable `TARGET`
	@echo ${TARGET}
.PHONY : target

build : ## Build image, for example, `./forge.mk build SERVICE=backend`
	docker build \
		--pull \
		--file "./${SERVICE}/Dockerfile.production" \
		--tag "${NAME}-${SERVICE}:${TARGET}" \
		"./${SERVICE}"
.PHONY : build

push : ## Push image, for example, `./forge.mk push SERVICE=backend USER=cloud HOST=192.102.163.92`
	docker save \
		--platform "linux/amd64" \
		"${NAME}-${SERVICE}:${TARGET}" \
		| bzip2 \
		| ssh "${USER}@${HOST}" "docker load"
.PHONY : push

all : ## Build and push all images
	$(MAKE) --file="${SELF}" \
		build \
		push \
		SERVICE=backend
	$(MAKE) --file="${SELF}" \
		build \
		push \
		SERVICE=frontend
.PHONY : all

# deploy : all ## Deploy in the cloud, for example, `./forge.mk deploy ENVIRONMENT=staging USER=cloud HOST=192.102.163.92`
# 	ssh "${USER}@${HOST}" " \
# 		cd '/app/${ENVIRONMENT}' \
# 		&& ./deploy.mk deploy TARGET='${TARGET}' \
# 	"
# .PHONY : deploy
