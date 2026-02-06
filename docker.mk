#!/usr/bin/env -S make --file

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

COMPOSE_BAKE=true

docker_compose = \
	docker compose \
		--env-file ./.env

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

name : ## Print value of variable `NAME`
	@echo ${NAME}
.PHONY : name

dotenv : ## Assert that all variables in ./.env.${ENVIRONMENT}.sample are available in ./.env
	${dotenv_linter} diff /mnt/.env "/mnt/.env.${ENVIRONMENT}.sample"
	${dotenv_linter} diff /mnt/frontend/.env.local "/mnt/frontend/.env.local.${ENVIRONMENT}.sample"
	${dotenv_linter} diff /mnt/.env.staging.sample /mnt/.env.production.sample
	${dotenv_linter} diff /mnt/frontend/.env.local.staging.sample /mnt/frontend/.env.local.production.sample
.PHONY : dotenv

config : ## Parse, resolve and render compose file in canonical format
	${docker_compose} config
.PHONY : config

check : ## Check build configuration
	${docker_compose} build \
		--check \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user)
.PHONY : check

pull : ## Pull images
	${docker_compose} pull
.PHONY : pull

# To debug errors during build add `--progress plain \` to get additional
# output.
build : dotenv check pull ## Build images
	${docker_compose} build \
		--pull \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user)
		# --no-cache
.PHONY : build

bake : ## Print docker-compose file equivalent bake file
	${docker_compose} build \
		--print \
		--pull
.PHONY : bake

build-context : ## Show the build context configured by `./${SERVICE}/.dockerignore`, for example, `make build-context SERVICE=backend`
	docker build \
		--pull \
		--no-cache \
		--progress plain \
		--file ./Dockerfile-show-build-context \
		./${SERVICE}
.PHONY : build-context

remove : ## Remove stopped services
	${docker_compose} rm \
		--volumes
.PHONY : remove

remove-data-volume : ## Remove data volume
	docker volume rm \
		${NAME}_data
.PHONY : remove-data

up : dotenv ## (Re)create and start services
	${docker_compose} up \
		--remove-orphans \
		--wait
.PHONY : up

down : ## Stop services and remove services and networks created by `up`
	${docker_compose} down \
		--remove-orphans
	-rm --force \
		./frontend/queries/*.generated.ts
.PHONY : down

restart : SERVICES =
restart : ## Restart all or specific stopped and running services, for example, `make restart` or `make restart SERVICES="database backend"`
	${docker_compose} restart ${SERVICES}
.PHONY : restart

attach : ## Attach to the `${SERVICE}` service, for example, `make attach SERVICE=backend` (to detach without stopping use `CTRL-p` followed by `CTRL-q` and otherwise `CTRL-c`)
	${docker_compose} attach ${SERVICE}
.PHONY : attach

prune : ## Remove all unused containers, unused networks, unused and dangling images, and unused anonymous volumes
	docker system prune \
		--volumes
.PHONY : prune

logs : ## Follow logs
	${docker_compose} logs \
		--since=1h \
		--follow
.PHONY : logs

exec : ## Execute the one-time command `${COMMAND}` against the `${SERVICE}` service
	${docker_compose} up \
		--remove-orphans \
		--wait \
		${SERVICE}
	${docker_compose} exec \
		${SERVICE} \
		${COMMAND}
.PHONY : exec

enter : COMMAND = bash
enter : exec ## Enter shell in the running `${SERVICE}` service, for example, `make enter SERVICE=database` or `make enter SERVICE=nginx`
.PHONY : enter

run : ## Run the one-time command `${COMMAND}` against a fresh `${SERVICE}` service
	${docker_compose} run \
		--rm \
		${SERVICE} \
		${COMMAND}
.PHONY : run

shell : COMMAND = bash
shell : run ## Enter shell in a fresh `${SERVICE}` service, for example, `make shell SERVICE=backend` or `make shell SERVICE=frontend`
.PHONY : shell

list : ## List all services with health status
	${docker_compose} ps \
		--no-trunc \
		--all
.PHONY : list

list-services : ## List all services specified in the docker-compose file (used by Monit as configured in the `machine` project)
	${docker_compose} config \
		--services
.PHONY : list-services
