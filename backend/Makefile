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
	dotnet tool run dotnet-format
.PHONY : format

assets : ## Build assets
	npm run all
.PHONY : assets

update : ## Update packages
	cd ./src/ && dotnet list package | awk '/>/ {print $$2}' | xargs -n 1 dotnet add package
	cd ./test/ && dotnet list package | awk '/>/ {print $$2}' | xargs -n 1 dotnet add package
.PHONY : update

# TODO Shortcuts for `dotnet aspnet-codegenerator`?

# List of commands
# https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

appmigrations : ## Add migration with name ${NAME} of application database context, for example, `make NAME=CreateProducts migrations`
	dotnet ef migrations add \
		--project src/Icon.csproj \
		--context ApplicationDbContext \
		--output-dir Data/Migrations/ApplicationDb \
		${NAME}
.PHONY : migrations

# http://docs.identityserver.io/en/latest/quickstarts/7_entity_framework.html
pgmigrations : ## Add migration with name ${NAME} of identity server's persisted grant database context, for example, `make NAME=CreateX migrations`
	dotnet ef migrations add \
		--project src/Icon.csproj \
		--context PersistedGrantDbContext \
		--output-dir Data/Migrations/IdentityServer/PersistedGrantDb \
		${NAME}
.PHONY : pgmigrations

# http://docs.identityserver.io/en/latest/quickstarts/7_entity_framework.html
cnfmigrations : ## Add migration with name ${NAME} of identity server's configuration database context, for example, `make NAME=CreateY migrations`
	dotnet ef migrations add \
		--project src/Icon.csproj \
		--context ConfigurationDbContext \
		--output-dir Data/Migrations/IdentityServer/ConfigurationDb \
		${NAME}
.PHONY : cnfmigrations

migrate : ## Run migrations
	dotnet ef database update \
		--project src/Icon.csproj \
		--context ApplicationDbContext
	dotnet ef database update \
		--project src/Icon.csproj \
		--context PersistedGrantDbContext
	dotnet ef database update \
		--project src/Icon.csproj \
		--context ConfigurationDbContext
.PHONY : migrate
