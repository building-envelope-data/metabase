# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

include .env

name = metabase

# Inspired by https://docs.docker.com/engine/reference/commandline/run/#add-entries-to-container-hosts-file---add-host
docker_ip = $(shell ip -4 addr show scope global dev docker0 | grep inet | awk '{print $$2}' | cut -d / -f 1)

docker_compose = \
	docker-compose \
		--file docker-compose.yml \
		--project-name ${name}

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

name : ## Print value of variable `name`
	@echo ${name}
.PHONY : name

# ----------------------------- #
# Interface with Docker Compose #
# ----------------------------- #

# TODO Try `buildkit` by setting the environment variables
# ```
# COMPOSE_DOCKER_CLI_BUILD=1 \
# DOCKER_BUILDKIT=1 \
# ```
# See https://docs.docker.com/develop/develop-images/build_enhancements/
# and https://www.docker.com/blog/faster-builds-in-compose-thanks-to-buildkit-support/
build : ## Build images
	DOCKER_IP=${docker_ip} \
		${docker_compose} build \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user)
.PHONY : build

show-backend-build-context : ## Show the build context configured by `./backend/.dockerignore`
	docker build --no-cache \
		--file Dockerfile-show-build-context \
		./backend
.PHONY : show-backend-build-context

show-frontend-build-context : ## Show the build context configured by `./frontend/.dockerignore`
	docker build --no-cache \
		--file Dockerfile-show-build-context \
		./frontend
.PHONY : show-frontend-build-context

remove : ## Remove stopped containers
	DOCKER_IP=${docker_ip} \
		${docker_compose} rm
.PHONY : remove

remove-data : ## Remove data volumes
	docker volume rm \
		${name}_data
.PHONY : remove-data

# TODO `docker-compose up` does not support `--user`, see https://github.com/docker/compose/issues/1532
up : build ## (Re)create, and start containers (after building images if necessary)
	DOCKER_IP=${docker_ip} \
		${docker_compose} up \
		--remove-orphans \
		--detach
.PHONY : up

down : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	DOCKER_IP=${docker_ip} \
		${docker_compose} down
.PHONY : down

restart : ## Restart all stopped and running containers
	DOCKER_IP=${docker_ip} \
		${docker_compose} restart
.PHONY : restart

logs : ## Follow logs
	DOCKER_IP=${docker_ip} \
		${docker_compose} logs \
		--follow
.PHONY : logs

exec : up ## Execute the one-time command `${COMMAND}` against an existing `${CONTAINER}` container (after starting all containers if necessary)
	DOCKER_IP=${docker_ip} \
		${docker_compose} exec \
		--user $(shell id --user):$(shell id --group) \
		${CONTAINER} \
		${COMMAND}
.PHONY : exec

execf : CONTAINER = frontend
execf : exec ## Execute the one-time command `${COMMAND}` against an existing `frontend` container (after starting all containers if necessary)
.PHONY : execf

execb : CONTAINER = backend
execb : exec ## Execute the one-time command `${COMMAND}` against an existing `backend` container (after starting all containers if necessary)
.PHONY : execb

shellf : COMMAND = ash -c "make install && exec ash"
shellf : execf ## Enter shell in an existing `frontend` container (after starting all containers if necessary)
.PHONY : shellf

shellb : COMMAND = ash
shellb : execb ## Enter shell in an existing `backend` container (after starting all containers if necessary)
.PHONY : shellb

shellb-examples : COMMAND = bash -c "cd ./examples && bash"
shellb-examples : execb ## Enter Bourne-again shell, aka, bash, in an existing `backend` container (after starting all containers if necessary)
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
		--dbname xbase_development
.PHONY : psql

shelld : up ## Enter shell in an existing `database` container (after starting all containers if necessary)
	DOCKER_IP=${docker_ip} \
		${docker_compose} exec \
		database \
		ash
.PHONY : shelld

createdb : ## Create databases
	DOCKER_IP=${docker_ip} \
		${docker_compose} exec \
		database \
		bash -c " \
			createdb --username postgres xbase_development ; \
		"
.PHONY : createdb

# ------ #
# Deploy #
# ------ #
# 1. Run `make register-*` to build (and register) the images.
# 2. Update the image names in `docker-compose.production.yml`.
# 3. Run `make deploy` to upload the images, Makefile,
#    docker-compose.production.yml files, and NGINX files to the server, and
#    to restart the services with the new configuration.

jump_host = sg.ise.fhg.de
remote_user="root"
remote_host="sx14666"

production_docker_compose = \
	docker-compose \
		--file docker-compose.production.yml \
		--project-name ${name}

# TODO Keep access token somewhere safe for example in an SSH vault.
login : ## Login as `${USER_NAME}` to the Fraunhofer registry with the access token in the file with path `${ACCESS_TOKEN}`, for example, `make USER_NAME=sim69815 ACCESS_TOKEN=./registry-access-token.txt login`
	cat ${ACCESS_TOKEN} | \
		docker login registry.gitlab.cc-asp.fraunhofer.de:4567 \
			--username ${USER_NAME} \
			--password-stdin
.PHONY : login

register : VERSION = $(shell git describe --tags | head --lines=1)
register : LOWER_PROJECT_NAME = $(shell echo ${PROJECT_NAME} | tr '[:upper:]' '[:lower:]')
register : REGISTRY_URL = registry.gitlab.cc-asp.fraunhofer.de:4567/ise621/icon
register : IMAGE = ${LOWER_PROJECT_NAME}-${END}-production
register : ## Build and register the production `${END}` image for project `${PROJECT_NAME}`
	docker build \
		--build-arg PROJECT_NAME=${PROJECT_NAME} \
		--tag ${REGISTRY_URL}/${IMAGE}:${VERSION} \
		--file Dockerfile-${END}-production .
	docker push ${REGISTRY_URL}/${IMAGE}:${VERSION}
.PHONY : register

register-backend : END = backend
register-backend : register ## Build and register backend image
.PHONY : register-backend

register-frontend : END = frontend
register-frontend : register ## Build and register frontend image
.PHONY : register-frontend

deploy : upload down-and-up-server ## Deploy, for example, `make JUMP_USER=swacker deploy`
.PHONY : deploy

upload : upload-images upload-files ## Upload images and files
.PHONY : upload

upload-images : ## Upload images
	for image in $(shell make --silent images-production | tr '\n' ' '); do \
		make \
			IMAGE=$${image} \
			JUMP_USER=${JUMP_USER} \
			upload-image-if-needed ; \
	done
.PHONY : upload-images

# Inspired by https://advancedweb.hu/deploying-docker-images-via-ssh/
upload-image-if-needed : REMOTE_IMAGE_ID = $(shell ssh -J ${JUMP_USER}@${jump_host} ${remote_user}@${remote_host} "docker images --format '{{.Repository}}:{{.Tag}} {{.ID}}' | grep ${IMAGE} | cut --delimiter ' ' --fields 2")
upload-image-if-needed : LOCAL_IMAGE_ID = $(shell docker images --format '{{.Repository}}:{{.Tag}} {{.ID}}' | grep ${IMAGE} | cut --delimiter ' ' --fields 2)
upload-image-if-needed : ## Upload image if needed
	if [ \
		"${REMOTE_IMAGE_ID}" \
		!= "${LOCAL_IMAGE_ID}" \
		] ; \
	then \
		make \
			IMAGE=${IMAGE} \
			JUMP_USER=${JUMP_USER} \
			upload-image ; \
  else \
		echo "Remote image ${IMAGE} is up-to-date" ; \
	fi
.PHONY : upload-image-if-needed

upload-image : ## Upload image
	docker save ${IMAGE} \
		| bzip2 \
		| pv \
		| ssh \
			-J ${JUMP_USER}@${jump_host} \
			${remote_user}@${remote_host} \
			'bunzip2 | docker load'
.PHONY : upload-image

images-production : ## Image names
	${production_docker_compose} config \
		| yq r - "services.*.image"
.PHONY : images-production

upload-files : ## Upload files
	scp \
		-r \
		-o "ProxyJump ${JUMP_USER}@${jump_host}" \
		./Makefile.production \
		./docker-compose.production.yml \
		./nginx \
		${remote_user}@${remote_host}:'~/app'
.PHONY : upload-files

down-and-up-server : ## Down and up server (note that a restart would not reflect configuration changes)
	ssh -t \
		-J ${JUMP_USER}@${jump_host} \
		${remote_user}@${remote_host} \
		" \
			cd ~/app && \
			make \
				--file Makefile.production \
				down \
				up && \
			exit \
		"
.PHONY : down-and-up-server

# --------------------- #
# Generate Certificates #
# --------------------- #

# TODO Pass passwords in a more secure way!
jwt-certificates : ## Create JWT encryption and signing certificates if necessary
	docker build \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user) \
		--tag ${name}_bootstrap \
		--file ./backend/Dockerfile-bootstrap \
		./backend
	docker run \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/backend",target=/app \
		${name}_bootstrap \
		ash -cx " \
			dotnet-script \
				create-certificates.csx \
				-- \
				${JSON_WEB_TOKEN_ENCRYPTION_CERTIFICATE_PASSWORD} \
				${JSON_WEB_TOKEN_SIGNING_CERTIFICATE_PASSWORD} \
		"
.PHONY : certificates

# For an introduction to how HTTPS works see https://howhttps.works
ssl : ## Generate and trust certificate authority, and generate SSL certificates
	make generate-certificate-authority
	make generate-ssl-certificate
	make trust-certificate-authority
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
		nginx:1.19.9 \
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
	DOCKER_IP=${docker_ip} \
		docker run \
		--user $(shell id --user):$(shell id --group) \
		--mount type=bind,source="$(shell pwd)/ssl",target=/ssl \
		nginx:1.19.9 \
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
	mkdir --parents ./backend/ssl
	cp ./ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.pfx ./backend/src/jwt-encryption-certificate.pfx
	cp ./ssl/${SSL_CERTIFICATE_BASE_FILE_NAME}.pfx ./backend/src/jwt-signing-certificate.pfx
.PHONY : generate-ssl-certificate

fetch-ssl-certificate : ## Fetch the SSL certificate of the server
	openssl s_client ${HOST}:${HTTPS_PORT}
.PHONY : fetch-ssl-certificate
