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

# TODO Strip the byte-order mark, aka, BOM
dos2unix : ## Remove carriage returns
	find src/ -type f -exec sed -i -e "s/\r//" {} +
	# find src/ -type f -exec sed -i -e "1s/^\xFEFF//" -e "s/\r//" {} +
	# find . -type f -exec dos2unix {} \;
	# find . \( -name "*.cs" -o -name "*.cshtml" \) ...

format : ## Format code
	dotnet-format

# TODO Shortcuts for `dotnet aspnet-codegenerator`?

# List of commands
# https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

migrations : ## Add migration with name ${NAME}, for example, `make NAME=CreateProducts migrations`
	dotnet ef migrations add \
		--context ApplicationDbContext \
		--output-dir src/Data/Migrations/ApplicationDb \
		${NAME}

migrate : ## Run migrations
	dotnet ef database update \
		--context ApplicationDbContext
	dotnet ef database update \
		--context PersistedGrantDbContext
	dotnet ef database update \
		--context ConfigurationDbContext
.PHONY : migrate
