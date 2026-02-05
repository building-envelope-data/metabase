#!/usr/bin/env -S make --file

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

docker_compose = \
	docker compose \
		--file ./docker-compose.yaml \
		--env-file ./.env

database_name = xbase

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
		--username postgres \
		--dbname ${database_name}
.PHONY : psql

# ${docker_compose} up \
# 	--remove-orphans \
# 	--wait \
# 	database
# ${docker_compose} exec \
# 	database \
# 	bash -c " \
# 		createdb --username postgres ${DBNAME} ; \
# 	"
createdb : DBNAME = ${database_name}
createdb : CONTAINER_NAME = create_${NAME}_database
createdb : ## Create database with name `${DBNAME}` defaulting to `xbase`
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
			--username postgres \
			${DBNAME}
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
.PHONY : createdb

dropdb : DBNAME = ${database_name}
dropdb : ## Drop database with name `${DBNAME}` defaulting to `xbase`
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
	${docker_compose} exec \
		database \
		dropdb \
			--username postgres \
			${DBNAME}
.PHONY : dropdb

sql : CONTAINER_NAME = sql_${NAME}_database
sql : ## Run the SQL script in the file `${SQL}` in the database service, for example, `make SQL=./my.sql sql` (down-ing and up-ing the database service before and after to prevent race conditions. In general, note that other PostgreSQL instances using the same data volume must not be used while migrating and need to be restarted afterwards to make migration results visible)
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
	cat ${SQL} \
	| ${docker_compose} exec \
		--no-tty \
		database \
		psql \
			--echo-all \
			--set=ON_ERROR_STOP=1 \
			--file=- \
			--username=postgres \
			--dbname=${database_name}
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
	cat ${SQL} \
	| docker exec \
		--no-tty \
		${CONTAINER_NAME} \
		psql \
			--echo-all \
			--set=ON_ERROR_STOP=1 \
			--file=- \
			--username=postgres \
			--dbname=${database_name}
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
.PHONY : sql

migrate : SQL = ./backend/src/Migrations/migrate.sql
migrate : sql ## Migrate database  by running the idempotent SQL script ./backend/src/Migrations/migrate.sql (down-ing and up-ing the database service before and after to prevent race conditions. In general, note that other PostgreSQL instances using the same data volume must not be used while migrating and need to be restarted afterwards to make migration results visible)
.PHONY : migrate

# Backup with `pg_dumpall`: https://www.postgresql.org/docs/13/backup-dump.html#BACKUP-DUMP-ALL
# Command `pg_dumpall`: https://www.postgresql.org/docs/13/app-pg-dumpall.html
backup : CONTAINER_NAME = backup_${NAME}_database
backup : ## Backup database and related data to directory with absolute path `${BACKUP_DIRECTORY}` (down-ing and up-ing the database service before and after to prevent race conditions), for example, `make BACKUP_DIRECTORY=./backups/$(date +"%Y-%m-%d_%H_%M_%S") backup`
	mkdir --parents ${BACKUP_DIRECTORY}
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
			--username=postgres \
		| gzip \
		> ${BACKUP_DIRECTORY}/${dump_archive_name}
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
.PHONY : backup

restore : CONTAINER_NAME = restore_${NAME}_database
restore : ## Restore database and related data from directory with absolute path `${BACKUP_DIRECTORY}` (down-ing and up-ing the database service before and after to prevent race conditions and removing and recreating the data volume before to start cleanly), for example, `make BACKUP_DIRECTORY=./backups/2021-04-22_15_43_35/ restore (note that after restoring a database it is usually necessary to restart the backend service for the object-relational mapper Npgsql to work seamlessly, for example, by restarting all services with `make restart`)`
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
	gunzip --stdout ${BACKUP_DIRECTORY}/${dump_archive_name} \
	| docker exec \
		--interactive \
		${CONTAINER_NAME} \
		psql \
			--echo-all \
			--set=ON_ERROR_STOP=1 \
			--file=- \
			--username=postgres \
			--dbname=postgres
	docker container stop ${CONTAINER_NAME}
	docker container rm --volumes ${CONTAINER_NAME}
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
.PHONY : restore

# TODO The entrypoint file of the PostgreSQL image uses the file refered to by `POSTGRES_PASSWORD_FILE` and this file needs to be accessible by `other`. Why? We do not want all people to be able to read secrets!
# https://www.postgresql.org/docs/current/libpq-pgpass.html
postgres_passwords : ## Generate PostgreSQL password and passwords files whose entries are nothing but one clear-text password or of the form `hostname:port:database:username:password` (note that if the data volume already exists, then it either needs to be removed resulting in the loss of all data or the password of the PostgreSQL user needs to be changed manually by executing the SQL query `ALTER USER postgres with password '...'` or running `\password postgres` within `make psql`)
	mkdir -p ./secrets
	chmod 0755 ./secrets
	touch ./secrets/postgres_password
	chmod 0644 ./secrets/postgres_password
	openssl rand -base64 32 \
		> ./secrets/postgres_password
	touch ./secrets/postgres_passwords
	chmod 0600 ./secrets/postgres_passwords
	echo "*:*:*:*:$$(cat ./secrets/postgres_password)" \
		> ./secrets/postgres_passwords
.PHONY : postgres_passwords
