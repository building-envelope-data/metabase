#!/usr/bin/env -S make --file
SELF := $(lastword $(MAKEFILE_LIST))

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

COMPOSE_BAKE=true

dump_archive_name = postgresql_dumpall.gz

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

psql : ## Enter PostgreSQL interactive terminal in the running `database` container
	docker compose up \
		--remove-orphans \
		--wait \
		database
	docker compose exec \
		database \
		psql \
		--username="${POSTGRES_USER}" \
		--dbname="${POSTGRES_DATABASE_NAME}"
.PHONY : psql

remove-volume : ## Remove data volume
	docker volume rm \
		"${NAME}_${ENVIRONMENT}_data"
.PHONY : remove-volume

create : CONTAINER_NAME = create_${NAME}_database
create : ## Create database with name `${POSTGRES_DATABASE_NAME}`
	-docker container stop "${CONTAINER_NAME}"
	-docker container rm --volumes "${CONTAINER_NAME}"
	docker compose run \
		--name "${CONTAINER_NAME}" \
		--detach \
		database
	while [[ "$$(docker inspect -f {{.State.Health.Status}} '${CONTAINER_NAME}')" != "healthy" ]]; do sleep 1; done
	docker exec \
		"${CONTAINER_NAME}" \
		createdb \
			--username="${POSTGRES_USER}" \
			"${POSTGRES_DATABASE_NAME}"
	docker container stop "${CONTAINER_NAME}"
	docker container rm --volumes "${CONTAINER_NAME}"
.PHONY : create

drop : CONTAINER_NAME = drop_${NAME}_database
drop : ## Drop database with name `${POSTGRES_DATABASE_NAME}`
	-docker container stop "${CONTAINER_NAME}"
	-docker container rm --volumes "${CONTAINER_NAME}"
	docker compose run \
		--name "${CONTAINER_NAME}" \
		--detach \
		database
	while [[ "$$(docker inspect -f {{.State.Health.Status}} '${CONTAINER_NAME}')" != "healthy" ]]; do sleep 1; done
	docker exec \
		"${CONTAINER_NAME}" \
		dropdb \
			--username="${POSTGRES_USER}" \
			"${POSTGRES_DATABASE_NAME}"
	docker container stop "${CONTAINER_NAME}"
	docker container rm --volumes "${CONTAINER_NAME}"
.PHONY : drop

sql : CONTAINER_NAME = sql_${NAME}_database
sql : ## Run the SQL script in the file `${SCRIPT}` in the database service, for example, `make sql SCRIPT=./my.sql ` (down-ing and up-ing the database service before and after to prevent race conditions. In general, note that other PostgreSQL instances using the same data volume must not be used while migrating and need to be restarted afterwards to make migration results visible)
	docker compose down \
		--remove-orphans \
		database
	-docker container stop "${CONTAINER_NAME}"
	-docker container rm --volumes "${CONTAINER_NAME}"
	docker compose run \
		--name "${CONTAINER_NAME}" \
		--detach \
		database
	while [[ "$$(docker inspect -f {{.State.Health.Status}} '${CONTAINER_NAME}')" != "healthy" ]]; do sleep 1; done
	cat "${SCRIPT}" \
	| docker exec \
		--interactive \
		"${CONTAINER_NAME}" \
		psql \
			--echo-all \
			--no-psqlrc \
			--set=ON_ERROR_STOP=on \
			--file=- \
			--username="${POSTGRES_USER}" \
			--dbname="${POSTGRES_DATABASE_NAME}"
	docker container stop "${CONTAINER_NAME}"
	docker container rm --volumes "${CONTAINER_NAME}"
	docker compose up \
		--remove-orphans \
		--wait \
		database
.PHONY : sql

migrate : SCRIPT = ./backend/src/Migrations/migrate.sql
migrate : sql ## Migrate database  by running the idempotent SQL script ./backend/src/Migrations/migrate.sql (down-ing and up-ing the database service before and after to prevent race conditions. In general, note that other PostgreSQL instances using the same data volume must not be used while migrating and need to be restarted afterwards to make migration results visible)
.PHONY : migrate

# Backup with `pg_dump`: https://www.postgresql.org/docs/current/backup-dump.html
# Command `pg_dump`: https://www.postgresql.org/docs/current/app-pgdump.html
backup : CONTAINER_NAME = backup_${NAME}_database
backup : ## Backup database and related data to directory with absolute path `${DIR}` (down-ing and up-ing the database service before and after to prevent race conditions), for example, `make backup DIR=./backups/$(date +"%Y-%m-%d_%H_%M_%S")`
	mkdir --parents "${DIR}"
	docker compose down \
		--remove-orphans \
		database
	-docker container stop "${CONTAINER_NAME}"
	-docker container rm --volumes "${CONTAINER_NAME}"
	docker compose run \
		--name "${CONTAINER_NAME}" \
		--detach \
		database
	while [[ "$$(docker inspect -f {{.State.Health.Status}} '${CONTAINER_NAME}')" != "healthy" ]]; do sleep 1; done
	docker exec \
		"${CONTAINER_NAME}" \
		pg_dump \
			--clean \
			--if-exists \
			--username="${POSTGRES_USER}" \
			--dbname="${POSTGRES_DATABASE_NAME}" \
		| gzip \
		> "${DIR}/${dump_archive_name}"
	docker container stop "${CONTAINER_NAME}"
	docker container rm --volumes "${CONTAINER_NAME}"
	docker compose up \
		--remove-orphans \
		--wait \
		database
.PHONY : backup

restore : CONTAINER_NAME = restore_${NAME}_database
restore : ## Restore database and related data from directory with absolute path `${DIR}` (down-ing and up-ing the database service before and after to prevent race conditions and dropping and recreating the database before to start cleanly), for example, `make restore DIR=./backups/2021-04-22_15_43_35/` (note that after restoring a database it is usually necessary to restart the backend service for the object-relational mapper Npgsql to work seamlessly, for example, by restarting all services with `make restart`)`
	docker compose down \
		--remove-orphans \
		database
	-docker container stop "${CONTAINER_NAME}"
	-docker container rm --volumes "${CONTAINER_NAME}"
	docker compose run \
		--name "${CONTAINER_NAME}" \
		--detach \
		database
	while [[ "$$(docker inspect -f {{.State.Health.Status}} '${CONTAINER_NAME}')" != "healthy" ]]; do sleep 1; done
	-docker exec \
		"${CONTAINER_NAME}" \
		dropdb \
			--username="${POSTGRES_USER}" \
			"${POSTGRES_DATABASE_NAME}"
	docker exec \
		"${CONTAINER_NAME}" \
		createdb \
			--username="${POSTGRES_USER}" \
			"${POSTGRES_DATABASE_NAME}"
	gunzip --stdout "${DIR}/${dump_archive_name}" \
	| docker exec \
		--interactive \
		"${CONTAINER_NAME}" \
		psql \
			--echo-all \
			--no-psqlrc \
			--set=ON_ERROR_STOP=on \
			--file=- \
			--username="${POSTGRES_USER}" \
			--dbname="${POSTGRES_DATABASE_NAME}"
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes "${CONTAINER_NAME}"
	docker compose up \
		--remove-orphans \
		--wait \
		database
.PHONY : restore
