#!/usr/bin/env -S make --file
SELF := $(lastword $(MAKEFILE_LIST))

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

COMPOSE_BAKE=true

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

do : ON_ERROR = pause
do : symlink ## Deploy tag, branch, or commit `${TARGET}`, for example, `./deploy.mk do TARGET=v1.0.0 ON_ERROR=pause`
	./deploy.sh target "${TARGET}" --on-error "${ON_ERROR}"
.PHONY : do

restore : ON_ERROR = pause
restore : symlink ## Restore deployment `${TIMESTAMP}`, for example, `./deploy.mk restore TIMESTAMP="2026-03-12T21:43:11+01:00" ON_ERROR=pause`
	./deploy.sh restore "${TIMESTAMP}" --on-error "${ON_ERROR}"
.PHONY : restore

resume : ON_ERROR = pause
resume : ## Resume a paused deployment attempt
	./deploy.sh resume --on-error "${ON_ERROR}"
.PHONY : resume

rollback : ## Rollback deployment attempt
	./deploy.sh rollback
.PHONY : rollback

state : ## Show deployment state
	./deploy.sh state
.PHONY : state

list : ## List deployments
	./deploy.sh list
.PHONY : list

begin-maintenance : ## Begin maintenance
	cp \
		./nginx/html/maintenance.off.html \
		./nginx/html/maintenance.html
.PHONY : begin-maintenance

end-maintenance : ## End maintenance
	rm --force \
		./nginx/html/maintenance.html
.PHONY : end-maintenance

store-target : ## Store the value `${TARGET}`
	echo "${TARGET}" \
		> ./.stored-target
.PHONY : store-target

set-target : ## Set variable `TARGET` in ./.env to `${TARGET}`
	sed --in-place --regexp-extended \
		's#^TARGET=(.*)$$#TARGET=${TARGET}#' \
		./.env
.PHONY : set-target

fetch-all : ## Fetch all
	git fetch --all
.PHONY : fetch-all

# Inspired by https://grimoire.ca/git/stop-using-git-pull-to-deploy/
switch : ## Switch to `${TARGET}`
	git switch \
		--discard-changes \
		--detach \
		"${TARGET}"
.PHONY : switch

dotenv : ## Assert that all variables in ./.env.${ENVIRONMENT}.sample are available in ./.env
	${dotenv_linter} diff /mnt/.env "/mnt/.env.${ENVIRONMENT}.sample"
.PHONY : dotenv

# Note that NGINX is because of its dependencies taken down and up last and in
# one go so the maintenance page is only down very shortly.
services : ## Recreate services
	docker compose up \
		--no-build \
		--no-deps \
		--force-recreate \
		--renew-anon-volumes \
		--remove-orphans \
		--wait
.PHONY : services

run-tests : ## TODO Run tests
.PHONY : run-tests

# $(MAKE) --file=./docker.mk \
# 	restart \
# 	SERVICE="${SERVICE}"
restart : ## Restart service `${SERVICE}` and await its health
	docker compose restart \
		--no-deps \
		${SERVICE}
.PHONY : restart

symlink : ## Confirm that ./Makefile links to ./docker.mk and that ./docker-compose.yaml links to the correct ./docker-compose.*.yaml
	if [[ ! -L "./Makefile" ]] || [[ ! "./Makefile" -ef "./docker.mk" ]]; then \
		echo "./docker-compose.yaml does not link to $${file}" >&2 ; \
		exit 1 ; \
	fi
	if [[ "${ENVIRONMENT}" == "staging" ]]; then \
		file="./docker-compose.production.yaml" ; \
	else \
		file="./docker-compose.${ENVIRONMENT}.yaml" ; \
	fi && \
	if [[ ! -L "./docker-compose.yaml" ]] || [[ ! "./docker-compose.yaml" -ef "$${file}" ]]; then \
		echo "./docker-compose.yaml does not link to $${file}" >&2 ; \
		exit 2 ; \
	fi
.PHONY : symlink
