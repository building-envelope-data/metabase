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
	GROUP_ID=$(shell id --group) \
		GROUP_NAME=$(shell id --group --name) \
		USER_ID=$(shell id --user) \
		USER_NAME=$(shell id --user --name) \
		docker-compose build
.PHONY : build

remove : ## Remove stopped containers
	docker-compose rm
.PHONY : remove

# TODO `docker-compose up` does not support `--user`, see https://github.com/docker/compose/issues/1532
up : build ## Build, (re)create, start, and attach to containers
	GROUP_ID=$(shell id --group) \
		GROUP_NAME=$(shell id --group --name) \
		USER_ID=$(shell id --user) \
		USER_NAME=$(shell id --user --name) \
		docker-compose up --remove-orphans

down : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose down

run : build ## Build the service `web` and run the one-time command `${COMMAND}` against it
	GROUP_ID=$(shell id --group) \
		GROUP_NAME=$(shell id --group --name) \
		USER_ID=$(shell id --user) \
		USER_NAME=$(shell id --user --name) \
		docker-compose run \
			--user $(shell id --user):$(shell id --group) \
			web \
			${COMMAND}
.PHONY : run

shell : COMMAND = ash
shell : run ## Enter shell
.PHONY : shell

migrations : COMMAND = python manage.py makemigrations
migrations : run ## Make migrations based on changes made to models
.PHONY : generate-migrations

migrate : COMMAND = python manage.py migrate
migrate : run ## Make and run migrations based on changes made to models
.PHONY : migrate

# ------------------------------------------------ #
# Tasks to run, for example, in a Docker container #
# ------------------------------------------------ #

tests : ## Run tests, for example, from a shell in a Docker container
	python -m pytest tests
.PHONY : tests

coverage : ## Generate test coverage report
	coverage run --source='.' manage.py test
	coverage report
.PHONY : coverage

seed : ## Seed the database
	python manage.py loaddata icon
