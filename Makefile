# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

name = icon

# Inspired by https://docs.docker.com/engine/reference/commandline/run/#add-entries-to-container-hosts-file---add-host
docker_ip = $(shell ip -4 addr show scope global dev docker0 | grep inet | awk '{print $$2}' | cut -d / -f 1)

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

build : ## Build images
	DOCKER_IP=${docker_ip} \
		docker-compose build \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user)
.PHONY : build

remove : ## Remove stopped containers
	DOCKER_IP=${docker_ip} \
		docker-compose rm
.PHONY : remove

# TODO `docker-compose up` does not support `--user`, see https://github.com/docker/compose/issues/1532
up : ## (Re)create and start containers
	DOCKER_IP=${docker_ip} \
		docker-compose up \
		--remove-orphans \
		--detach
.PHONY : up

up-ise : ## (Re)create and start containers
	docker-compose \
	  --file docker-compose-ise.yml \
		--project-name icon-ise \
		up \
		--remove-orphans \
		--detach
.PHONY : up-ise

up-lbnl : ## (Re)create and start containers
	docker-compose \
	  --file docker-compose-lbnl.yml \
		--project-name icon-lbnl \
		up \
		--remove-orphans \
		--detach
.PHONY : up-lbnl

down : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	DOCKER_IP=${docker_ip} \
		docker-compose down
.PHONY : down

down-ise : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose \
		--file docker-compose-ise.yml \
		--project-name icon-ise \
		down
.PHONY : down-ise

down-lbnl : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose \
		--file docker-compose-lbnl.yml \
		--project-name icon-lbnl \
		down
.PHONY : down-lbnl

restart : ## Restart all stopped and running containers
	DOCKER_IP=${docker_ip} \
		docker-compose restart
.PHONY : restart

restart-ise : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose \
		--file docker-compose-ise.yml \
		--project-name icon-ise \
		restart
.PHONY : restart-ise

restart-lbnl : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose \
		--file docker-compose-lbnl.yml \
		--project-name icon-lbnl \
		restart
.PHONY : restart-lbnl

logs : ## Follow logs
	DOCKER_IP=${docker_ip} \
		docker-compose logs \
		--follow
.PHONY : logs

logs-ise : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose \
		--file docker-compose-ise.yml \
		--project-name icon-ise \
		logs \
		--follow
.PHONY : logs-ise

logs-lbnl : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose \
		--file docker-compose-lbnl.yml \
		--project-name icon-lbnl \
		logs \
		--follow
.PHONY : logs-lbnl

runf : ## Run the one-time command `${COMMAND}` against a fresh `frontend` container
	DOCKER_IP=${docker_ip} \
		docker-compose run \
		--user $(shell id --user):$(shell id --group) \
		frontend \
		${COMMAND}
.PHONY : runf

runb : ## Run the one-time command `${COMMAND}` against a fresh `backend` container
	DOCKER_IP=${docker_ip} \
		docker-compose run \
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

psql : ## Enter PostgreSQL interactive terminal in the running `database` container
	DOCKER_IP=${docker_ip} \
		docker-compose exec \
		database \
		psql \
		--username postgres \
		--dbname icon_development
.PHONY : psql

# Inspired by https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust/59702094#59702094
# and https://superuser.com/questions/226192/avoid-password-prompt-for-keys-and-prompts-for-dn-information/226229#226229
# See also https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
# and https://github.com/dotnet/runtime/issues/31237#issuecomment-544929504
generate-https-certificate : ## Generate HTTPS certificate
	DOCKER_IP=${docker_ip} \
		docker run \
		--user $(shell id --user):$(shell id --group) \
		--tty \
		--interactive \
		--mount type=bind,source="$(shell pwd)/backend/https",target=/https \
		nginx \
		sh -c "openssl req -x509 -nodes -days 365 -newkey rsa:2048 -subj "/CN=localhost" -passout pass:password -keyout /https/localhost.key -out /https/localhost.crt -config /https/localhost.conf && openssl pkcs12 -export -out /https/localhost.pfx -inkey /https/localhost.key -in /https/localhost.crt && openssl verify -CAfile /https/localhost.crt /https/localhost.crt"

# Inspired by https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust/59702094#59702094
# See also https://github.com/dotnet/aspnetcore/issues/7246#issuecomment-541201757
# and https://github.com/dotnet/runtime/issues/31237#issuecomment-544929504
trust-https-certificate : ## Trust the generated HTTPS certificate
	sudo cp ./backend/https/localhost.crt /usr/local/share/ca-certificates
	sudo update-ca-certificates
	cat /etc/ssl/certs/ca-certificates.crt
	cat ./backend/https/localhost.crt
	sudo cat /etc/ssl/certs/localhost.pem
	openssl verify ./backend/https/localhost.crt

# ------------------------------------------------ #
# Tasks to run, for example, in a Docker container #
# ------------------------------------------------ #

diagrams : ## Draw images from textual UML diagrams
	plantuml diagrams/*.uml
.PHONY : diagrams
