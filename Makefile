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

# Executing with `--privileged` is necessary according to https://github.com/dotnet/diagnostics/blob/master/documentation/FAQ.md
traceb : ## Trace backend container with identifier `${CONTAINER_ID}`, for example, `make CONTAINER_ID=c1b82eb6e03c trace-backend`
	DOCKER_IP=${docker_ip} \
		${docker_compose} exec \
			--privileged \
			backend \
			ash -c " \
				make trace \
				"
.PHONY : traceb

psql : ## Enter PostgreSQL interactive terminal in the running `database` container
	DOCKER_IP=${docker_ip} \
		${docker_compose} exec \
		database \
		psql \
		--username postgres \
		--dbname icon_development
.PHONY : psql

ssl : generate-certificate-authority trust-certificate-authority ## Generate and trust certificate authority, and generate SSL certificates
	make generate-ssl-certificate
	make generate-ssl-certificate-ise
	make generate-ssl-certificate-lbnl
.PHONY : ssl

# Creating Self-Signed ECDSA SSL Certificate using OpenSSL: http://www.guyrutenberg.com/2013/12/28/creating-self-signed-ecdsa-ssl-certificate-using-openssl/
# See also https://gist.github.com/Soarez/9688998
# OpenSSL Quick Reference: https://www.digicert.com/kb/ssl-support/openssl-quick-reference-guide.htm
# X509v3 Extensions: See `man x509v3_config` and https://superuser.com/questions/738612/openssl-ca-keyusage-extension/1248085#1248085 and https://access.redhat.com/solutions/28965
generate-certificate-authority : ## Generate certificate authority ECDSA private key and self-signed certificate
	mkdir --parents ./ssl/
	DOCKER_IP=${docker_ip} \
		docker run \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/ssl",target=/ssl \
		nginx:1.19.0 \
		bash -cx " \
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
	mkdir --parents ./backend/ssl/
	cp ./ssl/${CERTIFICATE_AUTHORITY_BASE_FILE_NAME}.* ./backend/ssl/
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
	DOCKER_IP=${docker_ip} \
		docker run \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/ssl",target=/ssl \
		nginx:1.19.0 \
		bash -cx " \
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
	mkdir --parents ./backend/ssl/
	cp ./ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.* ./backend/ssl/
.PHONY : generate-ssl-certificate

generate-ssl-certificate-ise : HOST = ${ISE_HOST}
generate-ssl-certificate-ise : SSL_CERTIFICATE_BASE_FILE_NAME = ${ISE_SSL_CERTIFICATE_BASE_FILE_NAME}
generate-ssl-certificate-ise : SSL_CERTIFICATE_PASSWORD = ${ISE_SSL_CERTIFICATE_PASSWORD}
generate-ssl-certificate-ise : SSL_CERTIFICATE_SUBJECT = ${ISE_SSL_CERTIFICATE_SUBJECT}
generate-ssl-certificate-ise : generate-ssl-certificate ## Generate ISE SSL certificate
.PHONY : generate-ssl-certificate-ise

generate-ssl-certificate-lbnl : HOST = ${LBNL_HOST}
generate-ssl-certificate-lbnl : SSL_CERTIFICATE_BASE_FILE_NAME = ${LBNL_SSL_CERTIFICATE_BASE_FILE_NAME}
generate-ssl-certificate-lbnl : SSL_CERTIFICATE_PASSWORD = ${LBNL_SSL_CERTIFICATE_PASSWORD}
generate-ssl-certificate-lbnl : SSL_CERTIFICATE_SUBJECT = ${LBNL_SSL_CERTIFICATE_SUBJECT}
generate-ssl-certificate-lbnl : generate-ssl-certificate ## Generate LBNL SSL certificate
.PHONY : generate-ssl-certificate-lbnl

fetch-ssl-certificate : ## Fetch the SSL certificate of the server
	openssl s_client ${HOST}:${HTTPS_PORT}
.PHONY : fetch-ssl-certificate

fetch-ssl-certificate-ise : HOST = ${ISE_HOST}
fetch-ssl-certificate-ise : HTTPS_PORT = ${ISE_HTTPS_PORT}
fetch-ssl-certificate-ise : fetch-ssl-certificate ## Fetch the SSL certificate of the ISE server
.PHONY : fetch-ssl-certificate-ise

fetch-ssl-certificate-lbnl : HOST = ${LBNL_HOST}
fetch-ssl-certificate-lbnl : HTTPS_PORT = ${LBNL_HTTPS_PORT}
fetch-ssl-certificate-lbnl : fetch-ssl-certificate ## Fetch the SSL certificate of the LBNL server
.PHONY : fetch-ssl-certificate-lbnl

# ------------------------------------------------ #
# Tasks to run, for example, in a Docker container #
# ------------------------------------------------ #

diagrams : ## Draw images from textual UML diagrams
	plantuml diagrams/*.uml
.PHONY : diagrams
