#!/usr/bin/env -S make --file
SELF := $(lastword $(MAKEFILE_LIST))

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

COMPOSE_BAKE=true
SERVICE=

dotenv_linter = \
	docker run \
		--rm \
		--user $(shell id --user):$(shell id --group) \
		--volume "$(shell pwd):/mnt:ro" \
		--pull "always" \
		--quiet \
		dotenvlinter/dotenv-linter:latest

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

environment : ## Print value of variable `ENVIRONMENT`
	@echo ${ENVIRONMENT}
.PHONY : environment

symlink : ## Confirm that ./docker-compose.yaml links to the correct ./docker-compose.*.yaml
	if [[ ${ENVIRONMENT} == "staging" ]]; then \
		file="./docker-compose.production.yaml" ; \
	else \
		file="./docker-compose.${ENVIRONMENT}.yaml" ; \
	fi && \
	if [[ ! -L "./docker-compose.yaml" ]] || [[ ! "./docker-compose.yaml" -ef $${file} ]]; then \
	    echo "./docker-compose.yaml does not link to $${file}" ; \
	fi
.PHONY : symlink

dotenv : ## Assert that all variables in ./.env.${ENVIRONMENT}.sample are available in ./.env
	${dotenv_linter} diff /mnt/.env "/mnt/.env.${ENVIRONMENT}.sample"
	${dotenv_linter} diff /mnt/.env.staging.sample /mnt/.env.production.sample
.PHONY : dotenv

config : ## Parse, resolve and render compose file in canonical format
	docker compose config ${SERVICE}
.PHONY : config

check : ## Check build configuration
	docker compose build \
		--check \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user) ${SERVICE}
.PHONY : check

pull : ## Pull images
	docker compose pull ${SERVICE}
.PHONY : pull

build : symlink dotenv check pull ## Build images
	docker compose build \
		--pull \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user) ${SERVICE}
.PHONY : build

bake : ## Print docker-compose file equivalent bake file
	docker compose build \
		--print \
		--pull ${SERVICE}
.PHONY : bake

build-context : ## Show the build context configured by `./${SERVICE}/.dockerignore`, for example, `make build-context SERVICE=backend`
	docker build \
		--pull \
		--no-cache \
		--progress plain \
		--file ./Dockerfile.build-context \
		./${SERVICE}
.PHONY : build-context

remove : ## Remove stopped services
	docker compose rm \
		--volumes ${SERVICE}
.PHONY : remove

up : dotenv ## (Re)create and start services
	docker compose up \
		--no-build \
		--remove-orphans \
		--wait ${SERVICE}
.PHONY : up

down : ## Stop services and remove services and networks created by `up`
	docker compose down \
		--remove-orphans ${SERVICE}
	-rm --force \
		./frontend/queries/*.generated.ts
.PHONY : down

restart : ## Restart services (and await their health), for example, `make restart` to restart all services or `make restart SERVICE=nginx` or `make restart SERVICE="database backend"`
	docker compose restart ${SERVICE}
	docker compose up \
		--wait ${SERVICE}
.PHONY : restart

attach : ## Attach to the `${SERVICE}` service, for example, `make attach SERVICE=backend` (to detach without stopping use `CTRL-p` followed by `CTRL-q` and otherwise `CTRL-c`)
	docker compose attach ${SERVICE}
.PHONY : attach

prune : ## Remove all unused containers, unused networks, unused and dangling images, and unused anonymous volumes
	docker system prune \
		--volumes
.PHONY : prune

logs : ## Follow logs
	docker compose logs \
		--since=1h \
		--follow ${SERVICE}
.PHONY : logs

exec : ## Execute the one-time command `${COMMAND}` against the `${SERVICE}` service
	docker compose up \
		--remove-orphans \
		--wait \
		${SERVICE}
	docker compose exec \
		${SERVICE} \
		${COMMAND}
.PHONY : exec

enter : COMMAND = bash
enter : exec ## Enter shell in the running `${SERVICE}` service, for example, `make enter SERVICE=database` or `make enter SERVICE=nginx`
.PHONY : enter

run : ## Run the one-time command `${COMMAND}` against a fresh `${SERVICE}` service
	docker compose run \
		--rm \
		${SERVICE} \
		${COMMAND}
.PHONY : run

shell : COMMAND = bash
shell : run ## Enter shell in a fresh `${SERVICE}` service, for example, `make shell SERVICE=backend` or `make shell SERVICE=frontend`
.PHONY : shell

list : ## List services with health status
	docker compose ps \
		--no-trunc \
		--all ${SERVICE}
.PHONY : list

list-services : ## List all services specified in the docker-compose file (used by Monit as configured in the `machine` project)
	docker compose config \
		--services
.PHONY : list-services
