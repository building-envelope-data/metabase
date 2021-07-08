# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

# Taken from https://www.client9.com/self-documenting-makefiles/
help : ## Print this help
	@awk -F ':|##' '/^[^\t].+?:.*?##/ {\
		printf "\033[36m%-30s\033[0m %s\n", $$1, $$NF \
	}' $(MAKEFILE_LIST)
.PHONY : help
.DEFAULT_GOAL := help

# ------------------------------------------------ #
# Tasks to run, for example, in a Docker container #
# ------------------------------------------------ #

install : ## Install packages (without touching the lockfile)
	yarn install --frozen-lockfile
.PHONY : install

upgrade : ## Upgrade packages
	yarn upgrade
.PHONY : upgrade

outdated : ## Show outdated packages
	yarn outdated
.PHONY : outdated

lint : ## Lint code
	next lint
.PHONY : lint

update-schema : ## Update GraphQl schema
	wget \
		--output-document ./type-defs.graphqls \
		http://backend:8080/graphql?sdl
.PHONY : update-schema
