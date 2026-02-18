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

# To deploy `develop`, I usually just take all docker services down by running
# `cd /app/machine; make down; cd /app/production; make down; cd /app/staging; make down;`
# pull the latest code by running
# `cd /app/machine; git pull -p; cd /app/production; git pull -p; cd /app/staging; git pull -p;`
# redeploy the reverse proxy by running
# `cd /app/machine; make deploy;`
# backup, migrate, and deploy all services by running
# `./deploy.mk down backup migrate services DIR=$(pwd)/backup`
# first in `cd /app/staging` and checking that everything works as expected,
# and finally in `cd /app/production` and checking that everything works as
# expected. Before trying it on staging, I usually play the database from
# production into staging.
deploy : DIR = "$(shell pwd)/backup"
deploy : symlink ## Deploy tag, branch, or commit `${TARGET}`, for example, `./deploy.mk deploy TARGET=v1.0.0`
	$(MAKE) --file="${SELF}" begin-maintenance
	$(MAKE) --file="${SELF}" store-target set-target TARGET="${TARGET}"
	$(MAKE) --file="${SELF}" backup DIR="${DIR}"
	$(MAKE) --file="${SELF}" fetch-all
	$(MAKE) --file="${SELF}" checkout TARGET="${TARGET}"
	$(MAKE) --file="${SELF}" dotenv
	$(MAKE) --file="${SELF}" migrate
	$(MAKE) --file="${SELF}" services
	$(MAKE) --file="${SELF}" run-tests
	$(MAKE) --file="${SELF}" end-maintenance
.PHONY : deploy

rollback : TARGET = "$(shell cat ./.stored-target)"
rollback : DIR = "$(shell pwd)/backup"
rollback : symlink ## Rollback deployment attempt (uses target stored in `./.stored-target` and database backup stored in `./backup/`)
	$(MAKE) --file="${SELF}" begin-maintenance
	$(MAKE) --file="${SELF}" set-target TARGET="${TARGET}"
	$(MAKE) --file="${SELF}" checkout TARGET="${TARGET}"
	$(MAKE) --file="${SELF}" dotenv
	$(MAKE) --file="${SELF}" restore DIR="${DIR}"
	$(MAKE) --file="${SELF}" services
	$(MAKE) --file="${SELF}" run-tests
	$(MAKE) --file="${SELF}" end-maintenance
.PHONY : rollback

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

backup : ## Backup database and related data to the directory with the absolute path `${DIR}` (down-ing and up-ing the database service before and after to prevent race conditions), for example, `./deploy.mk backup DIR=/app/data/backups/$(date +"%Y-%m-%d_%H_%M_%S")`
	$(MAKE) --file=./database.mk \
		backup \
		DIR="${DIR}"
.PHONY : backup

restore : CONTAINER_NAME = restore_${NAME}_database
restore : ## Restore database and related data from the directory with the absolute path `${DIR}` (down-ing and up-ing the database service before and after to prevent race conditions and removing and recreating the data volume before to start cleanly), for example, `./deploy.mk restore DIR=/app/data/backups/2021-04-22_15_43_35/` (note that after restoring a database it is usually necessary to restart the backend service for the object-relational mapper Npgsql to work seamlessly, for example, by restarting all services with `./docker.mk restart`)`
	$(MAKE) --file=./database.mk \
		backup \
		DIR="${DIR}"
.PHONY : restore

migrate : ## Migrate database (down-ing and up-ing the database service before and after to prevent race conditions. In general, note that other PostgreSQL instances using the same data volume must not be used while migrating and need to be restarted afterwards to make migration results visible)
	$(MAKE) --file=./database.mk \
		migrate
.PHONY : migrate

fetch-all : ## Fetch all
	git fetch --all
.PHONY : fetch-all

# Inspired by https://grimoire.ca/git/stop-using-git-pull-to-deploy/
checkout : ## Fetch and checkout `${TARGET}`
	git checkout --force "${TARGET}"
.PHONY : checkout

dotenv : ## Assert that all variables in ./.env.${ENVIRONMENT}.sample are available in ./.env
	${dotenv_linter} diff /mnt/.env "/mnt/.env.${ENVIRONMENT}.sample"
.PHONY : dotenv

# Note that NGINX is because of its dependencies taken down and up last and in
# one go so the maintenance page is only down very shortly.
services : ## Recreate services
	docker compose up \
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
		${SERVICE}
.PHONY : restart

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
