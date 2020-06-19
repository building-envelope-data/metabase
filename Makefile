# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

include .env

name = icon
ise_name = ise_icon
lbnl_name = lbnl_icon

# Inspired by https://docs.docker.com/engine/reference/commandline/run/#add-entries-to-container-hosts-file---add-host
docker_ip = $(shell ip -4 addr show scope global dev docker0 | grep inet | awk '{print $$2}' | cut -d / -f 1)

docker_compose = \
	docker-compose \
		--file docker-compose.common.yml \
		--file docker-compose.yml \
		--project-name ${name}
ise_docker_compose = \
	docker-compose \
		--file docker-compose.common.yml \
		--file docker-compose.ise.yml \
		--project-name ${ise_name}
lbnl_docker_compose = \
	docker-compose \
		--file docker-compose.common.yml \
		--file docker-compose.lbnl.yml \
		--project-name ${lbnl_name}

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

# ----------------------------- #
# Interface with Docker Compose #
# ----------------------------- #

name : ## Print value of variable `name`
	@echo ${name}
.PHONY : name

name-ise : ## Print value of variable `ise_name`
	@echo ${ise_name}
.PHONY : name-ise

name-lbnl : ## Print value of variable `lbnl_name`
	@echo ${lbnl_name}
.PHONY : name-ise

build : ## Build images
	DOCKER_IP=${docker_ip} \
		${docker_compose} build \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user)
.PHONY : build

build-ise : docker_compose = ${ise_docker_compose}
build-ise : build ## Build ISE images
.PHONY : build-ise

build-lbnl : docker_compose = ${lbnl_docker_compose}
build-lbnl : build ## Build LBNL images
.PHONY : build-lbnl

remove : ## Remove stopped containers
	DOCKER_IP=${docker_ip} \
		${docker_compose} rm
.PHONY : remove

remove-ise : docker_compose = ${ise_docker_compose}
remove-ise : remove ## Remove stopped ISE containers
.PHONY : remove-ise

remove-lbnl : docker_compose = ${lbnl_docker_compose}
remove-lbnl : remove ## Remove stopped LBNL containers
.PHONY : remove-lbnl

remove-data : ## Remove all data volumes
	docker volume rm \
		${name}_data \
		${ise_name}_data \
		${lbnl_name}_data
.PHONY : remove-data

# TODO `docker-compose up` does not support `--user`, see https://github.com/docker/compose/issues/1532
up : ## (Re)create and start containers
	DOCKER_IP=${docker_ip} \
		${docker_compose} up \
		--remove-orphans \
		--detach
.PHONY : up

up-ise : docker_compose = ${ise_docker_compose}
up-ise : up ## (Re)create and start ISE containers
.PHONY : up-ise

up-lbnl : docker_compose = ${lbnl_docker_compose}
up-lbnl : up ## (Re)create and start LBNL containers
.PHONY : up-lbnl

down : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	DOCKER_IP=${docker_ip} \
		${docker_compose} down
.PHONY : down

down-ise : docker_compose = ${ise_docker_compose}
down-ise : down ## Stop ISE containers and remove containers, networks, volumes, and images created by `up-ise`
.PHONY : down-ise

down-lbnl : docker_compose = ${lbnl_docker_compose}
down-lbnl : down ## Stop LBNL containers and remove containers, networks, volumes, and images created by `up-lbnl`
.PHONY : down-lbnl

restart : ## Restart all stopped and running containers
	DOCKER_IP=${docker_ip} \
		${docker_compose} restart
.PHONY : restart

restart-ise : docker_compose = ${ise_docker_compose}
restart-ise : restart ## Restart all stopped and running ISE containers
.PHONY : restart-ise

restart-lbnl : docker_compose = ${lbnl_docker_compose}
restart-lbnl : restart ## Restart all stopped and running LBNL containers
.PHONY : restart-lbnl

logs : ## Follow logs
	DOCKER_IP=${docker_ip} \
		${docker_compose} logs \
		--follow
.PHONY : logs

logs-ise : docker_compose = ${ise_docker_compose}
logs-ise : logs ## Follow ISE logs
.PHONY : logs-ise

logs-lbnl : docker_compose = ${lbnl_docker_compose}
logs-lbnl : logs ## Follow LBNL logs
.PHONY : logs-lbnl

runf : ## Run the one-time command `${COMMAND}` against a fresh `frontend` container
	DOCKER_IP=${docker_ip} \
		${docker_compose} run \
		--user $(shell id --user):$(shell id --group) \
		frontend \
		${COMMAND}
.PHONY : runf

runb : ## Run the one-time command `${COMMAND}` against a fresh `backend` container
	DOCKER_IP=${docker_ip} \
		${docker_compose} run \
		--user $(shell id --user):$(shell id --group) \
		backend \
		${COMMAND}
.PHONY : runb

shellf : COMMAND = ash
shellf : runf ## Enter shell in a fresh `frontend` container
.PHONY : shellf

shellb : COMMAND = ash
shellb : runb ## Enter shell in a fresh `backend` container
.PHONY : shellb

shellb-examples : COMMAND = bash -c "cd ./examples && bash"
shellb-examples : runb ## Enter Bourne-again shell, aka, bash, in a fresh `backend` container
.PHONY : shellb-examples

psql : ## Enter PostgreSQL interactive terminal in the running `database` container
	DOCKER_IP=${docker_ip} \
		${docker_compose} exec \
		database \
		psql \
		--username postgres \
		--dbname icon_development
.PHONY : psql

# Creating Self-Signed ECDSA SSL Certificate using OpenSSL: http://www.guyrutenberg.com/2013/12/28/creating-self-signed-ecdsa-ssl-certificate-using-openssl/
# `-param_enc explicit` tells openssl to embed the full parameters of the curve in the key, as opposed to just its name
generate-certificate-authority : ## Generate certificate authority ECDSA private key and self-signed certificate
	DOCKER_IP=${docker_ip} \
		docker run \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/ssl",target=/ssl \
		nginx \
		sh -c " \
			echo \"# Generate the eliptic curve (EC) private key '/ssl/ca.key' with parameters 'secp521r1', that is, a NIST/SECG curve over a 521 bit prime field as said in the output of the command 'openssl ecparam -list_curves'\" && \
			openssl ecparam \
				-genkey \
				-name secp521r1 \
				-param_enc explicit \
				-out /ssl/ca.key && \
			echo \"# Generate the certificate request '/ssl/ca.req' with common name 'ca.org' from the private key\" && \
			openssl req \
				-new \
				-subj \"/CN=ca.org\" \
				-key /ssl/ca.key \
				-out /ssl/ca.req && \
			echo \"# Verify the request\" && \
			openssl req \
				-verify \
				-noout \
				-in /ssl/ca.req && \
			echo \"# Convert the request into the self-signed certificate '/ssl/ca.crt'\" && \
			openssl x509 \
				-req \
				-trustout \
				-days 365 \
				-in /ssl/ca.req \
				-signkey /ssl/ca.key \
				-out /ssl/ca.crt && \
			echo \"# Verify the self-signed certificate\" && \
			openssl verify \
				-CAfile /ssl/ca.crt \
				/ssl/ca.crt \
			"
	mkdir --parents ./backend/ssl/
	cp ./ssl/ca.* ./backend/ssl/

# Inspired by https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust/59702094#59702094
# See also https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
# and https://github.com/dotnet/runtime/issues/31237#issuecomment-544929504
trust-certificate-authority : ## Trust the generated SSL certificate
	sudo cp ./ssl/ca.crt /usr/local/share/ca-certificates
	sudo update-ca-certificates
	cat /etc/ssl/certs/ca-certificates.crt
	cat ./ssl/ca.crt
	sudo cat /etc/ssl/certs/ca.pem
	openssl verify ./ssl/ca.crt
.PHONY : trust-certificate-authority

# Inspired by https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust/59702094#59702094
# and https://superuser.com/questions/226192/avoid-password-prompt-for-keys-and-prompts-for-dn-information/226229#226229
# See also https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
# and https://github.com/dotnet/runtime/issues/31237#issuecomment-544929504
# For an explanation of the distinction between `cert` and `pfx` files, see
# https://security.stackexchange.com/questions/29425/difference-between-pfx-and-cert-certificates/29428#29428
# `-param_enc explicit` tells openssl to embed the full parameters of the curve in the key, as opposed to just its name
generate-ssl-certificate : ## Generate SSL certificate
	DOCKER_IP=${docker_ip} \
		docker run \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/ssl",target=/ssl \
		nginx \
		sh -cx " \
			echo \"# Generate the eliptic curve (EC) private key '/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key' with parameters 'secp521r1', that is, a NIST/SECG curve over a 521 bit prime field as said in the output of the command 'openssl ecparam -list_curves'\" && \
			openssl ecparam \
				-genkey \
				-name secp521r1 \
				-param_enc explicit \
				-out /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key && \
			echo \"# Generate the PKCS#10 certificate request '/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.req' with common name '${HOST}' from the private key\" && \
			openssl req \
				-new \
				-subj \"/CN=${HOST}\" \
				-key /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key \
				-out /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.req && \
			echo \"# Sign the request with certificate authority '/ssl/ca.crt' and key '/ssl/ca.key' resulting in the signed certificate '/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt'\" && \
			openssl x509 \
				-req \
				-days 365 \
				-CA /ssl/ca.crt \
				-CAkey /ssl/ca.key \
				-CAcreateserial \
				-in /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.req \
				-out /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt && \
			echo \"# Verify the signed certificate\" && \
			openssl verify \
				-CAfile /ssl/ca.crt \
				/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt && \
			echo \"# Create the PKCS#12 file '/ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.pfx' from the signed certificate\" && \
			openssl pkcs12 \
				-export \
				-passout pass:${SSL_CERTIFICATE_PASSWORD} \
				-in /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.crt \
				-inkey /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.key \
				-out /ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.pfx \
			"
	mkdir --parents ./backend/ssl/
	cp ./ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.* ./backend/ssl/
.PHONY : generate-ssl-certificate

generate-ssl-certificate-ise : HOST = ${ISE_HOST}
generate-ssl-certificate-ise : SSL_CERTIFICATE_BASE_FILE_NAME = ${ISE_SSL_CERTIFICATE_BASE_FILE_NAME}
generate-ssl-certificate-ise : SSL_CERTIFICATE_PASSWORD = ${ISE_SSL_CERTIFICATE_PASSWORD}
generate-ssl-certificate-ise : generate-ssl-certificate ## Generate ISE SSL certificate
.PHONY : generate-ssl-certificate-ise

generate-ssl-certificate-lbnl : HOST = ${LBNL_HOST}
generate-ssl-certificate-lbnl : SSL_CERTIFICATE_BASE_FILE_NAME = ${LBNL_SSL_CERTIFICATE_BASE_FILE_NAME}
generate-ssl-certificate-lbnl : SSL_CERTIFICATE_PASSWORD = ${LBNL_SSL_CERTIFICATE_PASSWORD}
generate-ssl-certificate-lbnl : generate-ssl-certificate ## Generate LBNL SSL certificate
.PHONY : generate-ssl-certificate-lbnl

# ------------------------------------------------ #
# Tasks to run, for example, in a Docker container #
# ------------------------------------------------ #

diagrams : ## Draw images from textual UML diagrams
	plantuml diagrams/*.uml
.PHONY : diagrams
