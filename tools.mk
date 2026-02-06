#!/usr/bin/env -S make --file

include ./.env

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

COMPOSE_BAKE=true

docker_compose = \
	docker compose \
		--env-file ./.env

dotenv_linter = \
	docker run \
		--rm \
		--user $(shell id --user):$(shell id --group) \
		--volume "$(shell pwd):/mnt:ro" \
		--pull "always" \
		--quiet \
		dotenvlinter/dotenv-linter:latest

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

# Executing with `--privileged` is necessary according to https://github.com/dotnet/diagnostics/blob/master/documentation/FAQ.md
trace-backend : ## Trace backend service with identifier `${CONTAINER_ID}`, for example, `make CONTAINER_ID=c1b82eb6e03c trace-backend`
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

dclint = \
	docker run \
		--rm \
		--tty \
		--user $(shell id --user):$(shell id --group) \
		--volume "$(shell pwd):/app" \
		--pull "always" \
		--quiet \
		zavoloklom/dclint:latest \
		--config /app/.dclintrc

hadolint = \
	docker run \
		--rm \
		--interactive \
		--user $(shell id --user):$(shell id --group) \
		--volume ./.hadolint.yaml:/.config/.hadolint.yaml \
		--pull "always" \
		--quiet \
		hadolint/hadolint:latest \
		hadolint \
		--config /.config/.hadolint.yaml

# docker run \
# 	--workdir / \
# 	--volume ./checkmake.ini:/checkmake.ini \
# 	--volume ./Makefile.development:/Makefile.development \
# 	--volume ./Makefile.production:/Makefile.production \
# 	--volume ./backend/Makefile:/Makefile.backend \
# 	--volume ./frontend/Makefile:/Makefile.frontend \
# 	quay.io/checkmake/checkmake \
# 	/Makefile.development \
# 	/Makefile.production \
# 	/Makefile.backend \
# 	/Makefile.frontend
lint : ## Lint .env files, Docker Compose files, and Dockerfiles
	@echo Lint .env Files
	${dotenv_linter} \
		check \
		--recursive \
		--ignore-checks UnorderedKey \
		.
	@echo Lint Docker Compose Files
	${dclint} .
	@echo Lint Dockerfiles
	for dockerfile in $(shell find . -name "Dockerfile*"); do \
		echo "... $${dockerfile}" \
		${hadolint} - < $${dockerfile} ; \
	done
.PHONY : lint

fix : ## Fix .env files and Docker Compose linting violations
	@echo Fix .env Files
	${dotenv_linter} \
		fix \
		--no-backup \
		--recursive \
		--ignore-checks UnorderedKey \
		.
	@echo Fix Docker Compose Files
	${dclint} --fix .
.PHONY : fix

format : ## Format Dockerfiles
	docker run \
		--rm \
		--user $(shell id --user):$(shell id --group) \
		--volume "$(shell pwd):/pwd" \
		--pull "always" \
		--quiet \
		ghcr.io/reteps/dockerfmt:latest \
		--indent 2 \
		--newline \
		--write \
		$(shell find . -name "Dockerfile*" -printf "/pwd/%h/%f ")
.PHONY : format

diagrams-plantuml : ## Draw images from textual UML diagrams
	plantuml diagrams/plantuml/*.puml
.PHONY : diagrams-plantuml

# `diagrams-structurizr starts a server which can be accessed with a browser at localhost:9090. The diagrams can be downloaded manually from there.
diagrams-structurizr : ## Serve diagrams to browser localhost Port 9090
	docker run -it --rm -p 9090:8080 -v "$(shell pwd)/diagrams/structurizr:/usr/local/structurizr structurizr/lite"
.PHONY : diagrams-structurizr
