# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

docker_compose = \
	docker compose \
		--file ./docker-compose.yml \
		--env-file ./.env \
		--project-name ${NAME}

database_name = xbase

dump_archive_name = postgresql_dumpall.gz

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

dotenv : ## Assert that all variables in `./.env.sample` are available in `./.env`
	bash -c " \
		diff \
			<(cut --only-delimited --delimiter='=' --fields=1 ./.env.sample | sort) \
			<(cut --only-delimited --delimiter='=' --fields=1 ./.env        | sort) \
	"
	bash -c " \
		diff \
			<(cut --only-delimited --delimiter='=' --fields=1 ./frontend/.env.local.sample | sort) \
			<(cut --only-delimited --delimiter='=' --fields=1 ./frontend/.env.local        | sort) \
	"
	bash -c " \
		diff \
			<(cut --only-delimited --delimiter='=' --fields=1 ./.env.production.sample | sort) \
			<(cut --only-delimited --delimiter='=' --fields=1 ./.env.staging.sample    | sort) \
	"
	bash -c " \
		diff \
			<(cut --only-delimited --delimiter='=' --fields=1 ./frontend/.env.local.production.sample | sort) \
			<(cut --only-delimited --delimiter='=' --fields=1 ./frontend/.env.local.staging.sample    | sort) \
	"
.PHONY : dotenv

# ----------------------------- #
# Interface with Docker Compose #
# ----------------------------- #

config : ## Parse, resolve and render compose file in canonical format
	COMPOSE_BAKE=true \
		COMPOSE_DOCKER_CLI_BUILD=1 \
			DOCKER_BUILDKIT=1 \
				${docker_compose} config
.PHONY : config

check : ## Check build configuration
	COMPOSE_BAKE=true \
		COMPOSE_DOCKER_CLI_BUILD=1 \
			DOCKER_BUILDKIT=1 \
				${docker_compose} build \
					--check \
					--build-arg GROUP_ID=$(shell id --group) \
					--build-arg USER_ID=$(shell id --user)
.PHONY : check

pull : ## Pull images
	COMPOSE_BAKE=true \
		COMPOSE_DOCKER_CLI_BUILD=1 \
			DOCKER_BUILDKIT=1 \
				${docker_compose} pull
.PHONY : pull

# To debug errors during build add `--progress plain \` to get additional
# output.
build : dotenv check pull ## Build images
	COMPOSE_BAKE=true \
		COMPOSE_DOCKER_CLI_BUILD=1 \
			DOCKER_BUILDKIT=1 \
				${docker_compose} build \
					--pull \
					--build-arg GROUP_ID=$(shell id --group) \
					--build-arg USER_ID=$(shell id --user)
.PHONY : build

build-bootstrap : ## Build the bootstrap image
	DOCKER_BUILDKIT=1 \
		docker build \
			--pull \
			--build-arg GROUP_ID=$(shell id --group) \
			--build-arg USER_ID=$(shell id --user) \
			--tag ${NAME}_bootstrap \
			--file ./backend/Dockerfile-bootstrap \
			./backend
.PHONY : build-bootstrap

bake : ## Print docker-compose file equivalent bake file
	COMPOSE_BAKE=true \
		COMPOSE_DOCKER_CLI_BUILD=1 \
			DOCKER_BUILDKIT=1 \
				${docker_compose} build \
					--print \
					--pull
					# --no-cache
.PHONY : bake

backend-build-context : ## Show the build context configured by `./backend/.dockerignore`
	DOCKER_BUILDKIT=1 \
		docker build \
			--pull \
			--no-cache \
			--progress plain \
			--file Dockerfile-show-build-context \
			./backend
.PHONY : backend-build-context

frontend-build-context : ## Show the build context configured by `./frontend/.dockerignore`
	DOCKER_BUILDKIT=1 \
		docker build \
			--pull \
			--no-cache \
			--progress plain \
			--file Dockerfile-show-build-context \
			./frontend
.PHONY : frontend-build-context

remove : ## Remove stopped containers
	${docker_compose} rm \
		--volumes
.PHONY : remove

remove-data : ## Remove data volumes
	docker volume rm \
		${NAME}_data
.PHONY : remove-data

# TODO `docker compose up` does not support `--user`, see https://github.com/docker/compose/issues/1532
up : build ## (Re)create, and start containers (after building images if necessary)
	${docker_compose} up \
		--remove-orphans \
		--wait
.PHONY : up

upb : ## (Re)create, start, and attach to backend container (to detach without stopping use `CTRL-p` followed by `CTRL-q` and otherwise `CTRL-c`)
	${docker_compose} up \
		--remove-orphans \
		backend
.PHONY : upb

down : ## Stop containers and remove containers and networks created by `up` and clear backend logs
	${docker_compose} down \
		--remove-orphans
	-rm ./frontend/queries/*.generated.ts
.PHONY : down

restart : ## Restart all stopped and running containers
	${docker_compose} restart
.PHONY : restart

restartb : ## Restart the backend container
	${docker_compose} restart backend
.PHONY : restartb

attachb : ## Attach to the backend container (to detach without stopping use `CTRL-p` followed by `CTRL-q` and otherwise `CTRL-c`)
	${docker_compose} attach backend
.PHONY : attachb

prune : ## Remove all unused containers, unused networks, unused and dangling images, and unused anonymous volumes
	docker system prune \
		--volumes
.PHONY : prune

logs : ## Follow logs
	${docker_compose} logs \
		--since=1h \
		--follow
.PHONY : logs

exec : ## Execute the one-time command `${COMMAND}` against the `${CONTAINER}` container
	${docker_compose} up \
		--remove-orphans \
		--wait \
		${CONTAINER}
	${docker_compose} exec \
		--user $(shell id --user):$(shell id --group) \
		${CONTAINER} \
		${COMMAND}
.PHONY : exec

execf : CONTAINER = frontend
execf : exec ## Execute the one-time command `${COMMAND}` against the `frontend` container
.PHONY : execf

execb : CONTAINER = backend
execb : exec ## Execute the one-time command `${COMMAND}` against the `backend` container
.PHONY : execb

run : ## Run the one-time command `${COMMAND}` against a fresh `${CONTAINER}` container
	${docker_compose} run \
		--rm \
		--user $(shell id --user):$(shell id --group) \
		${CONTAINER} \
		${COMMAND}
.PHONY : run

runf : CONTAINER = frontend
runf : run ## Run the one-time command `${COMMAND}` against a fresh `frontend` container
.PHONY : runf

runb : CONTAINER = backend
runb : run ## runute the one-time command `${COMMAND}` against a fresh `backend` container
.PHONY : runb

shellf : COMMAND = bash
shellf : execf ## Enter shell in the `frontend` container
.PHONY : shellf

shellb : COMMAND = bash
shellb : runb ## Enter shell in a fresh `backend` container
.PHONY : shellb

shellb-examples : COMMAND = bash -c "cd ./examples && bash"
shellb-examples : runb ## Enter Bourne-again shell, aka, bash, in the `backend` container
.PHONY : shellb-examples

# Executing with `--privileged` is necessary according to https://github.com/dotnet/diagnostics/blob/master/documentation/FAQ.md
traceb : ## Trace backend container with identifier `${CONTAINER_ID}`, for example, `make CONTAINER_ID=c1b82eb6e03c trace-backend`
	${docker_compose} up \
		--remove-orphans \
		--wait \
		backend
	${docker_compose} exec \
			--privileged \
			backend \
			bash -c " \
				make trace \
				"
.PHONY : traceb

shelln : ## Enter shell in the `nginx` container
	${docker_compose} up \
		--remove-orphans \
		--wait \
		nginx
	${docker_compose} exec \
		nginx \
		bash
.PHONY : shelln

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

shelld : CONTAINER = database
shelld : COMMAND = bash
shelld : exec ## Enter shell in the `database` container
.PHONY : shelld

list : ## List all containers with health status
	${docker_compose} ps \
		--no-trunc \
		--all
.PHONY : list

dclint = \
	docker run \
		--rm \
		--tty \
		--user $(shell id --user):$(shell id --group) \
		--volume .:/app \
		--pull "always" \
		zavoloklom/dclint:latest \
		--config /app/.dclintrc

hadolint = \
	docker run \
		--rm \
		--interactive \
		--user $(shell id --user):$(shell id --group) \
		--volume ./.hadolint.yml:/.config/.hadolint.yaml \
		--pull "always" \
		hadolint/hadolint:latest \
		hadolint \
		--config /.config/.hadolint.yaml

# docker run \
# 	--workdir / \
# 	--volume ./checkmake.ini:/checkmake.ini \
# 	--volume ./Makefile:/Makefile \
# 	--volume ./Makefile.production:/Makefile.production \
# 	--volume ./backend/Makefile:/Makefile.backend \
# 	--volume ./frontend/Makefile:/Makefile.frontend \
# 	quay.io/checkmake/checkmake \
# 	/Makefile \
# 	/Makefile.production \
# 	/Makefile.backend \
# 	/Makefile.frontend
lint : ## Lint Docker Compose  and Dockerfiles
	@echo Lint Docker Compose Files
	${dclint} .
	@echo Lint Dockerfiles
	for dockerfile in $(shell find . -name "Dockerfile*"); do \
		echo "... $${dockerfile}" \
		${hadolint} - < $${dockerfile} ; \
	done
.PHONY : lint

fix : ## Fix Docker Compose linting violations
	${dclint} --fix .
.PHONY : fix

format : ## Format Dockerfiles
	docker run \
		--rm \
		--user $(shell id --user):$(shell id --group) \
		--volume $(shell pwd):/pwd \
		--pull "always" \
		ghcr.io/reteps/dockerfmt:latest \
		--indent 2 \
		--newline \
		--write \
		$(shell find . -name "Dockerfile*" -printf "/pwd/%h/%f ")
.PHONY : format

createdb : DBNAME = ${database_name}
createdb : ## Create database with name `${DBNAME}` defaulting to `xbase`
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
	${docker_compose} exec \
		database \
		bash -c " \
			createdb --username postgres ${DBNAME} ; \
		"
.PHONY : createdb

dropdb : DBNAME = ${database_name}
dropdb : ## Drop database with name `${DBNAME}` defaulting to `xbase`
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
	${docker_compose} exec \
		database \
		bash -c " \
			dropdb --username postgres ${DBNAME} ; \
		"
.PHONY : dropdb

sql : ## Run the SQL script in the file `${SQL}` in the running `database` service, for example, `make SQL=./my.sql sql`
	${docker_compose} up \
		--remove-orphans \
		--wait \
		database
	cat ${SQL} \
	| ${docker_compose} exec \
		--no-TTY \
		database \
		psql \
			--echo-all \
			--set=ON_ERROR_STOP=1 \
			--file=- \
			--username=postgres \
			--dbname=${database_name}
.PHONY : sql

migrate : SQL = ./backend/src/Migrations/migrate.sql
migrate : sql ## Migrate the database by running the idempotent SQL script ./backend/src/Migrations/migrate.sql
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

begin-maintenance : ## Begin maintenance
	cp \
		./nginx/html/maintenance.off.html \
		./nginx/html/maintenance.html
.PHONY : begin-maintenance

end-maintenance : ## End maintenance
	rm ./nginx/html/maintenance.html
.PHONY : begin-maintenance

diagrams-plantuml : ## Draw images from textual UML diagrams
	plantuml diagrams/plantuml/*.puml
.PHONY : diagrams-plantuml

# `diagrams-structurizr starts a server which can be accessed with a browser at localhost:9090. The diagrams can be downloaded manually from there.
diagrams-structurizr : ## Serve diagrams to browser localhost Port 9090
	docker run -it --rm -p 9090:8080 -v $(shell pwd)/diagrams/structurizr:/usr/local/structurizr structurizr/lite
.PHONY : diagrams-structurizr

# --------------------- #
# Generate Certificates #
# --------------------- #

# TODO Pass passwords in a more secure way!
jwt-certificates : build-bootstrap ## Create JWT encryption and signing certificates if necessary
	docker run \
		--rm \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/backend",target=/app \
		${NAME}_bootstrap \
		bash -ceux " \
			dotnet-script \
				/app/create-certificates.csx \
				-- \
				${JSON_WEB_TOKEN_ENCRYPTION_CERTIFICATE_PASSWORD} \
				${JSON_WEB_TOKEN_SIGNING_CERTIFICATE_PASSWORD} \
		"
.PHONY : jwt-certificates

# For an introduction to how HTTPS works see https://howhttps.works
ssl : ## Generate and trust certificate authority, and generate SSL certificates
	$(MAKE) generate-certificate-authority
	$(MAKE) generate-ssl-certificate
	$(MAKE) trust-certificate-authority
.PHONY : ssl

# Creating Self-Signed ECDSA SSL Certificate using OpenSSL: http://www.guyrutenberg.com/2013/12/28/creating-self-signed-ecdsa-ssl-certificate-using-openssl/
# See also https://gist.github.com/Soarez/9688998
# OpenSSL Quick Reference: https://www.digicert.com/kb/ssl-support/openssl-quick-reference-guide.htm
# X509v3 Extensions: See `man x509v3_config` and https://superuser.com/questions/738612/openssl-ca-keyusage-extension/1248085#1248085 and https://access.redhat.com/solutions/28965
# Which file extensions are meant for which file layout? https://serverfault.com/questions/9708/what-is-a-pem-file-and-how-does-it-differ-from-other-openssl-generated-key-file/9717#9717
generate-certificate-authority : ## Generate certificate authority ECDSA private key and self-signed certificate
	mkdir --parents ./ssl/
		docker run \
		--rm \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/ssl",target=/ssl \
		nginx:1.29-trixie-otel \
		bash -ceux " \
			echo \"# Generate the elliptic curve (EC) private key '/ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.key' with parameters 'secp384r1', that is, a NIST/SECG curve over a 384 bit prime field as said in the output of the command 'openssl ecparam -list_curves'\" && \
			openssl ecparam \
				-genkey \
				-name secp384r1 \
				-out /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.key && \
			echo \"# Check and print the private key's elliptic curve parameters\" && \
			openssl ecparam \
				-check \
				-text \
				-in /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.key \
				-noout && \
			echo \"# Generate the PKCS#10 certificate request '/ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.req' with common name '${CERTIFICATE_AUTHORITY_HOST}' from the private key\" && \
			openssl req \
				-new \
				-subj \"${CERTIFICATE_AUTHORITY_SUBJECT}/CN=${CERTIFICATE_AUTHORITY_HOST}\" \
				-key /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.key \
				-out /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.req && \
			echo \"# Verify and print the request\" && \
			openssl req \
				-verify \
				-text \
				-in /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.req \
				-noout && \
			echo \"# Convert the request into the self-signed certificate '/ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt'\" && \
			openssl x509 \
				-req \
				-trustout \
				-days 365 \
				-extfile <(printf ' \
					basicConstraints = critical, CA:TRUE, pathlen:0\n \
					subjectKeyIdentifier = hash\n \
					authorityKeyIdentifier = keyid:always, issuer:always\n \
					subjectAltName = DNS:${CERTIFICATE_AUTHORITY_HOST}\n \
					issuerAltName = issuer:copy\n \
					keyUsage = critical, cRLSign, digitalSignature, keyCertSign\n \
				') \
				-in /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.req \
				-signkey /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.key \
				-out /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt && \
			echo \"# Print and Verify the self-signed certificate\" && \
			openssl x509 \
				-text \
				-purpose \
				-in /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
				-noout && \
			openssl verify \
				-CAfile /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
				/ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt && \
			echo \"# Create the PKCS#12 file '/ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.pfx' from the self-signed certificate\" && \
			openssl pkcs12 \
				-export \
				-passout pass:${CERTIFICATE_AUTHORITY_PASSWORD} \
				-in /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
				-inkey /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.key \
				-out /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.pfx && \
			echo \"# Verify the PKCS#12 file\" && \
			( \
				openssl pkcs12 \
					-info \
					-passin pass:${CERTIFICATE_AUTHORITY_PASSWORD} \
					-in /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.pfx \
					-noout && \
				echo \"PKCS#12 file is valid\" && \
				exit 0 \
			) || echo \"PFX file is invalid\" \
			"
	mkdir --parents ./backend/ssl
	cp ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.* ./backend/ssl
	mkdir --parents ./frontend/ssl
	cp ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.* ./frontend/ssl
.PHONY : generate-certificate-authority

# Inspired by https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust/59702094#59702094
# See also https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
# and https://github.com/dotnet/runtime/issues/31237#issuecomment-544929504
# and https://superuser.com/questions/437330/how-do-you-add-a-certificate-authority-ca-to-ubuntu/719047#719047
# For debugging purposes, the following commands can be helpful
# cat /etc/ssl/certs/ca-certificates.crt
# cat ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt
# sudo cat /etc/ssl/certs/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.pem
# Note that Firefox and Google Chrome use their own certificate stores:
# * Firefox: https://www.cyberciti.biz/faq/firefox-adding-trusted-ca/
# * Google Chrome: https://rshankar.com/blog/2010/07/08/how-to-import-a-certificate-in-google-chrome/
trust-certificate-authority : ## Trust the authority's SSL certificate
	sudo cp ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt /usr/local/share/ca-certificates
	sudo update-ca-certificates
	openssl verify ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt
.PHONY : trust-certificate-authority

# Inspired by https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust/59702094#59702094
# and https://superuser.com/questions/226192/avoid-password-prompt-for-keys-and-prompts-for-dn-information/226229#226229
# See also https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
# and https://github.com/dotnet/runtime/issues/31237#issuecomment-544929504
# For an explanation of the distinction between `cert` and `pfx` files, see
# https://security.stackexchange.com/questions/29425/difference-between-pfx-and-cert-certificates/29428#29428
# OpenSSL Quick Reference: https://www.digicert.com/kb/ssl-support/openssl-quick-reference-guide.htm
# X509v3 Extensions: See `man x509v3_config` and https://superuser.com/questions/738612/openssl-ca-keyusage-extension/1248085#1248085 and https://access.redhat.com/solutions/28965
# What are PKCS#12 files? https://security.stackexchange.com/questions/29425/difference-between-pfx-and-cert-certificates/29428#29428
# Process substitution `<( ... )`: https://www.gnu.org/software/bash/manual/html_node/Process-Substitution.html
# Note that extensions are not transferred to certificate requests and vice versa as said on https://www.openssl.org/docs/man1.1.0/man1/x509.html#BUGS
generate-ssl-certificate : ## Generate ECDSA private key and SSL certificate signed by our certificate authority
	mkdir --parents ./ssl/
	docker run \
		--rm \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/ssl",target=/ssl \
		nginx:1.29-trixie-otel \
		bash -ceux " \
			echo \"# Generate the elliptic curve (EC) private key '/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key' with parameters 'secp384r1', that is, a NIST/SECG curve over a 384 bit prime field as said in the output of the command 'openssl ecparam -list_curves'\" && \
			openssl ecparam \
				-genkey \
				-name secp384r1 \
				-out /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key && \
			echo \"# Check and print the private key's elliptic curve parameters\" && \
			openssl ecparam \
				-check \
				-text \
				-in /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key \
				-noout && \
			echo \"# Generate the PKCS#10 certificate request '/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.req' with common name '${HOST}' from the private key\" && \
			openssl req \
				-new \
				-subj \"${SSL_CERTIFICATE_SUBJECT}/CN=${HOST}\" \
				-key /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key \
				-out /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.req && \
			echo \"# Verify and print the request\" && \
			openssl req \
				-verify \
				-text \
				-in /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.req \
				-noout && \
			echo \"# Sign the request with certificate authority '/ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt' and key '/ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.key' resulting in the signed certificate '/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt'\" && \
			openssl x509 \
				-req \
				-days 365 \
				-CA /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
				-CAkey /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.key \
				-CAcreateserial \
				-extfile <(printf ' \
					basicConstraints = critical, CA:FALSE\n \
					subjectKeyIdentifier = hash\n \
					authorityKeyIdentifier = keyid:always, issuer:always\n \
					subjectAltName = DNS:${HOST}\n \
					issuerAltName = issuer:copy\n \
					keyUsage = critical, nonRepudiation, digitalSignature, keyEncipherment, keyAgreement\n \
					extendedKeyUsage = critical, clientAuth, serverAuth\n \
				') \
				-in /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.req \
				-out /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt && \
			echo \"# Print and Verify the signed certificate\" && \
			openssl x509 \
				-text \
				-purpose \
				-in /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt \
				-noout && \
			openssl verify \
				-CAfile /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
				/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt && \
			echo \"# Chain the certificate and the certificate authority's certificate in that order, see http://nginx.org/en/docs/http/configuring_https_servers.html#chains\" && \
			cat \
				/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt \
				/ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
				> /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.chained.crt && \
			echo \"# Verify the chained certificate\" && \
			openssl verify \
				-CAfile /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
				/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.chained.crt && \
			echo \"# Create the PKCS#12 file chain '/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.pfx' from the un-chained signed certificate\" && \
			openssl pkcs12 \
				-export \
				-chain \
				-CAfile /ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt \
				-passout pass:${SSL_CERTIFICATE_PASSWORD} \
				-in /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt \
				-inkey /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key \
				-out /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.pfx && \
			echo \"# Verify the PKCS#12 file\" && \
			( \
				openssl pkcs12 \
					-info \
					-passin pass:${SSL_CERTIFICATE_PASSWORD} \
					-in /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.pfx \
					-noout && \
				echo \"PKCS#12 file is valid\" && \
				exit 0 \
			) || echo \"PFX file is invalid\" \
			"
.PHONY : generate-ssl-certificate

fetch-ssl-certificate : ## Fetch the SSL certificate of the server
	openssl s_client ${HOST}:${HTTPS_PORT}
.PHONY : fetch-ssl-certificate

ssl-certificate : ## Print the SSL certificate
	openssl x509 -text -noout -in ./ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt
.PHONY : ssl-certificate

certificate-authority : ## View the certificate authority
	openssl x509 -text -noout -in ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.crt
.PHONY : certificate-authority
