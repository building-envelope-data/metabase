#!/usr/bin/env -S make --file

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

COMPOSE_BAKE=true

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
deploy : DIR = $(shell pwd)/backup
deploy : ## Deploy tag, branch, or commit `${TARGET}`, for example, `make deploy TARGET=v1.0.0`
	$(MAKE) begin-maintenance
	$(MAKE) store-commit
	$(MAKE) backup
	$(MAKE) fetch-all
	$(MAKE) checkout-target
	$(MAKE) migrate
	$(MAKE) services
	$(MAKE) run-tests
	$(MAKE) end-maintenance
.PHONY : deploy

rollback : TARGET = $(shell cat ./commit)
rollback : DIR = $(shell pwd)/backup
rollback : ## Rollback deployment attempt (use commit hash stored in `./commit` and database backup stored in `./backup/`)
	$(MAKE) begin-maintenance
	$(MAKE) checkout-target
	$(MAKE) restore
	$(MAKE) services
	$(MAKE) run-tests
	$(MAKE) end-maintenance
.PHONY : rollback

begin-maintenance : ## Begin maintenance
	cp \
		./nginx/html/maintenance.off.html \
		./nginx/html/maintenance.html
.PHONY : begin-maintenance

end-maintenance : ## End maintenance
	rm ./nginx/html/maintenance.html
.PHONY : end-maintenance

backup : ## Backup database and related data to the directory with the absolute path `${DIR}` (down-ing and up-ing the database service before and after to prevent race conditions), for example, `make backup DIR=/app/data/backups/$(date +"%Y-%m-%d_%H_%M_%S")`
	$(MAKE) \
		--file="$(shell pwd)/database.mk" \
		DIR=${DIR} \
		backup
.PHONY : backup

restore : CONTAINER_NAME = restore_${NAME}_database
restore : ## Restore database and related data from the directory with the absolute path `${DIR}` (down-ing and up-ing the database service before and after to prevent race conditions and removing and recreating the data volume before to start cleanly), for example, `make restore DIR=/app/data/backups/2021-04-22_15_43_35/` (note that after restoring a database it is usually necessary to restart the backend service for the object-relational mapper Npgsql to work seamlessly, for example, by restarting all services with `make restart`)`
	$(MAKE) \
		--file="$(shell pwd)/database.mk" \
		DIR=${DIR} \
		backup
.PHONY : restore

migrate : ## Migrate database (down-ing and up-ing the database service before and after to prevent race conditions. In general, note that other PostgreSQL instances using the same data volume must not be used while migrating and need to be restarted afterwards to make migration results visible)
	$(MAKE) \
		--file="$(shell pwd)/database.mk" \
		migrate
.PHONY : migrate

store-commit : ## Store current commit
	git rev-parse \
		--verify \
		HEAD \
	> ./commit
.PHONY : store-commit

fetch-all : ## Fetch all
	git fetch --all
.PHONY : fetch-all

# Inspired by https://grimoire.ca/git/stop-using-git-pull-to-deploy/
checkout-target : ## Fetch and checkout `${TARGET}`
	git checkout --force "${TARGET}"
.PHONY : checkout-target

# Note that NGINX is because of its dependencies taken down and up last and in
# one go so the maintenance page is only down very shortly.
services : ## Deploy services
	$(MAKE) build
	docker compose up \
		--force-recreate \
		--renew-anon-volumes \
		--remove-orphans \
		--wait
.PHONY : services

run-tests : COMMAND = true
run-tests : execb ## Run tests
.PHONY : run-tests
