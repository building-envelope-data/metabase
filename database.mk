#!/usr/bin/env -S make --file

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

docker_compose = \
	docker compose \
		--file ./docker-compose.yaml \
		--env-file ./.env

dump_archive_name = postgresql_dumpall.gz

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

psql : ## Enter PostgreSQL interactive terminal in the running `database` container
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
	${docker_compose} exec \
		database \
		psql \
		--username="${POSTGRES_USER}" \
		--dbname="${POSTGRES_DATABASE_NAME}"
.PHONY : psql

createdb : CONTAINER_NAME = create_${NAME}_database
createdb : ## Create database with name `${POSTGRES_DATABASE_NAME}`
	-docker container stop ${CONTAINER_NAME}
	-docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} run \
		--name ${CONTAINER_NAME} \
		--detach \
		database
	while [ $$(docker inspect -f {{.State.Health.Status}} ${CONTAINER_NAME}) != "healthy" ]; do sleep 1; done
	docker exec \
		${CONTAINER_NAME} \
		createdb \
			--username="${POSTGRES_USER}" \
			${POSTGRES_DATABASE_NAME}
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
.PHONY : createdb

dropdb : CONTAINER_NAME = drop_${NAME}_database
dropdb : ## Drop database with name `${POSTGRES_DATABASE_NAME}`
	-docker container stop ${CONTAINER_NAME}
	-docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} run \
		--name ${CONTAINER_NAME} \
		--detach \
		database
	while [ $$(docker inspect -f {{.State.Health.Status}} ${CONTAINER_NAME}) != "healthy" ]; do sleep 1; done
	docker exec \
		${CONTAINER_NAME} \
		dropdb \
			--username="${POSTGRES_USER}" \
			${POSTGRES_DATABASE_NAME}
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
.PHONY : dropdb

sql : CONTAINER_NAME = sql_${NAME}_database
sql : ## Run the SQL script in the file `${SCRIPT}` in the database service, for example, `make sql SCRIPT=./my.sql ` (down-ing and up-ing the database service before and after to prevent race conditions. In general, note that other PostgreSQL instances using the same data volume must not be used while migrating and need to be restarted afterwards to make migration results visible)
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
	cat ${SCRIPT} \
	| ${docker_compose} exec \
		--no-tty \
		database \
		psql \
			--echo-all \
			--set=ON_ERROR_STOP=1 \
			--file=- \
			--username="${POSTGRES_USER}" \
			--dbname=${POSTGRES_DATABASE_NAME}
	${docker_compose} down \
		--remove-orphans \
		database
	-docker container stop ${CONTAINER_NAME}
	-docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} run \
		--name ${CONTAINER_NAME} \
		--detach \
		database
	while [ $$(docker inspect -f {{.State.Health.Status}} ${CONTAINER_NAME}) != "healthy" ]; do sleep 1; done
	cat ${SCRIPT} \
	| docker exec \
		--no-tty \
		${CONTAINER_NAME} \
		psql \
			--echo-all \
			--set=ON_ERROR_STOP=1 \
			--file=- \
			--username="${POSTGRES_USER}" \
			--dbname=${POSTGRES_DATABASE_NAME}
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
.PHONY : sql

migrate : SCRIPT = ./backend/src/Migrations/migrate.sql
migrate : sql ## Migrate database  by running the idempotent SQL script ./backend/src/Migrations/migrate.sql (down-ing and up-ing the database service before and after to prevent race conditions. In general, note that other PostgreSQL instances using the same data volume must not be used while migrating and need to be restarted afterwards to make migration results visible)
.PHONY : migrate

# Backup with `pg_dumpall`: https://www.postgresql.org/docs/13/backup-dump.html#BACKUP-DUMP-ALL
# Command `pg_dumpall`: https://www.postgresql.org/docs/13/app-pg-dumpall.html
backup : CONTAINER_NAME = backup_${NAME}_database
backup : ## Backup database and related data to directory with absolute path `${DIR}` (down-ing and up-ing the database service before and after to prevent race conditions), for example, `make backup DIR=./backups/$(date +"%Y-%m-%d_%H_%M_%S")`
	mkdir --parents ${DIR}
	${docker_compose} down \
		--remove-orphans \
		database
	-docker container stop ${CONTAINER_NAME}
	-docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} run \
		--name ${CONTAINER_NAME} \
		--detach \
		database
	while [ $$(docker inspect -f {{.State.Health.Status}} ${CONTAINER_NAME}) != "healthy" ]; do sleep 1; done
	docker exec \
		${CONTAINER_NAME} \
		pg_dumpall \
			--clean \
			--username="${POSTGRES_USER}" \
		| gzip \
		> ${DIR}/${dump_archive_name}
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
.PHONY : backup

restore : CONTAINER_NAME = restore_${NAME}_database
restore : ## Restore database and related data from directory with absolute path `${DIR}` (down-ing and up-ing the database service before and after to prevent race conditions and removing and recreating the data volume before to start cleanly), for example, `make restore DIR=./backups/2021-04-22_15_43_35/` (note that after restoring a database it is usually necessary to restart the backend service for the object-relational mapper Npgsql to work seamlessly, for example, by restarting all services with `make restart`)`
	${docker_compose} down \
		--remove-orphans \
		database
	docker volume rm \
		${NAME}_data
	-docker container stop ${CONTAINER_NAME}
	-docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} run \
		--name ${CONTAINER_NAME} \
		--detach \
		database
	while [ $$(docker inspect -f {{.State.Health.Status}} ${CONTAINER_NAME}) != "healthy" ]; do sleep 1; done
	gunzip --stdout ${DIR}/${dump_archive_name} \
	| docker exec \
		--interactive \
		${CONTAINER_NAME} \
		psql \
			--echo-all \
			--set=ON_ERROR_STOP=1 \
			--file=- \
			--username="${POSTGRES_USER}" \
			--dbname="${POSTGRES_DATABASE_NAME}"
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
.PHONY : restore
