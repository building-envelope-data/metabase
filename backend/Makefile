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
# TODO Apply not only to `src` folder but ignore files and directories within .gitignore.
#      With git installed there are various options, which all have some
#      disadvantages, see
#      https://unix.stackexchange.com/questions/358270/find-files-that-are-not-in-gitignore
dos2unix : ## Remove carriage returns
	find src/ -type f -exec sed -i -e "s/\r//" {} +
	# find src/ -type f -exec sed -i -e "1s/^\xFEFF//" -e "s/\r//" {} +
	# find . -type f -exec dos2unix {} \;
	# find . \( -name "*.cs" -o -name "*.cshtml" \) ...
.PHONY : dos2unix

format : ## Format code
	dotnet-format
.PHONY : format

assets : ## Build assets
	npm run all
.PHONY : assets

# TODO Shortcuts for `dotnet aspnet-codegenerator`?

# List of commands
# https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

migrations : ## Add migration with name ${NAME}, for example, `make NAME=CreateProducts migrations`
	dotnet ef migrations add \
		--context ApplicationDbContext \
		--output-dir src/Data/Migrations/ApplicationDb \
		${NAME}
.PHONY : migrations

migrate : ## Run migrations
	dotnet ef database update \
		--context ApplicationDbContext
	dotnet ef database update \
		--context PersistedGrantDbContext
	dotnet ef database update \
		--context ConfigurationDbContext
.PHONY : migrate
