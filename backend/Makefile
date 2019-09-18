# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

migrations : ## Make migrations according to ${ARGS} based on changes made to models
	npm run -- migration:generate ${ARGS}
.PHONY : migrations

migrate : ## Run migrations based on changes made to models
	npm run -- migration:run
.PHONY : migrate

rollback : ## Rollback migrations
	npm run -- migration:revert
.PHONY : rollback
