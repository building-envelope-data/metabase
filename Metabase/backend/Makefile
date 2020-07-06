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

assets : ## Build assets
	npm run all
.PHONY : assets

# https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-trace-instructions.md#using-dotnet-trace
trace : ## Trace the dotnet process `Metabase`
	dotnet tool run \
		dotnet-trace collect \
			--process-id $$(dotnet tool run dotnet-trace ps | grep Metabase | awk '{print $$1}') \
			--format speedscope \
			--providers Microsoft-Windows-DotNETRuntime
.PHONY : trace

# https://github.com/JosefPihrt/Roslynator/blob/master/docs/cli/README.md
analyze : ## Analyze with Roslynator
	dotnet tool run roslynator analyze

play : ## Run facts in `Test.Play` (semi-alternative to the buggy `dotnet script` REPL)
	dotnet test --filter "FullyQualifiedName~Test.Play"
.PHONY : play

# TODO Shortcuts for `dotnet aspnet-codegenerator`?

# List of commands
# https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

appmigrations : ## Add migration with name ${NAME} of application database context, for example, `make NAME=CreateProducts migrations`
	dotnet ef migrations add \
		--project src/Metabase.csproj \
		--context ApplicationDbContext \
		--output-dir Data/Migrations/ApplicationDb \
		${NAME}
.PHONY : migrations

# http://docs.identityserver.io/en/latest/quickstarts/7_entity_framework.html
pgmigrations : ## Add migration with name ${NAME} of identity server's persisted grant database context, for example, `make NAME=CreateX migrations`
	dotnet ef migrations add \
		--project src/Metabase.csproj \
		--context PersistedGrantDbContext \
		--output-dir Data/Migrations/IdentityServer/PersistedGrantDb \
		${NAME}
.PHONY : pgmigrations

# http://docs.identityserver.io/en/latest/quickstarts/7_entity_framework.html
cnfmigrations : ## Add migration with name ${NAME} of identity server's configuration database context, for example, `make NAME=CreateY migrations`
	dotnet ef migrations add \
		--project src/Metabase.csproj \
		--context ConfigurationDbContext \
		--output-dir Data/Migrations/IdentityServer/ConfigurationDb \
		${NAME}
.PHONY : cnfmigrations

migrate : ## Run migrations
	dotnet ef database update \
		--project src/Metabase.csproj \
		--context ApplicationDbContext
	dotnet ef database update \
		--project src/Metabase.csproj \
		--context PersistedGrantDbContext
	dotnet ef database update \
		--project src/Metabase.csproj \
		--context ConfigurationDbContext
.PHONY : migrate
