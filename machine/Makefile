# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

docker_compose = \
	docker-compose \
		--file docker-compose.yml

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

deploy : ## Deploy services
	${docker_compose} pull
	${docker_compose} up \
		--build \
		--force-recreate \
		--renew-anon-volumes \
		--remove-orphans \
		--detach
.PHONY : deploy

logs : ## Follow logs
	${docker_compose} logs \
		--follow
.PHONY : logs

down : ## Stop containers and remove containers, networks, volumes, and images created by `deploy`
	${docker_compose} down \
		--remove-orphans
.PHONY : down
