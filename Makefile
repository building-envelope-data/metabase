# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

name = icon

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
	docker-compose build \
		--build-arg GROUP_ID=$(shell id --group) \
		--build-arg USER_ID=$(shell id --user)
.PHONY : build

remove : ## Remove stopped containers
	docker-compose rm
.PHONY : remove

# TODO `docker-compose up` does not support `--user`, see https://github.com/docker/compose/issues/1532
up : ## (Re)create and start containers
	docker-compose up \
		--remove-orphans \
		--detach
.PHONY : up

down : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose down
.PHONY : down

restart : ## Restart all stopped and running containers
	docker-compose restart
.PHONY : restart

logs : ## Follow logs
	docker-compose logs \
		--follow
.PHONY : logs

runf : ## Run the one-time command `${COMMAND}` against a fresh `frontend` container
	docker-compose run \
		--user $(shell id --user):$(shell id --group) \
		frontend \
		${COMMAND}
.PHONY : runf

runb : ## Run the one-time command `${COMMAND}` against a fresh `backend` container
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
