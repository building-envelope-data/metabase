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
remove : ## Remove stopped containers
	docker-compose rm
.PHONY : remove

# TODO `docker-compose up` does not support `--user`, see https://github.com/docker/compose/issues/1532
up : ## (Re)create and start containers
	docker-compose up \
		--remove-orphans \
		--detach

down : ## Stop containers and remove containers, networks, volumes, and images created by `up`
	docker-compose down

restart : ## Restart all stopped and running containers
	docker-compose restart

logs : ## Follow logs
	docker-compose logs \
		--follow

run : ## Run the one-time command `${COMMAND}` against it
	docker-compose run \
		--user $(shell id --user):$(shell id --group) \
		backend \
		${COMMAND}
.PHONY : run

shell : COMMAND = ash
shell : run ## Enter shell
.PHONY : shell

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
