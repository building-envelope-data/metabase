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

# TODO Apply not only to `src` folder but ignore files and directories within .gitignore.
#      With git installed there are various options, which all have some
#      disadvantages, see
#      https://unix.stackexchange.com/questions/358270/find-files-that-are-not-in-gitignore
# Inspired by
# * https://stackoverflow.com/questions/45240387/how-can-i-remove-the-bom-from-a-utf-8-file/45240995#45240995
# * https://unix.stackexchange.com/questions/381230/how-can-i-remove-the-bom-from-a-utf-8-file/381263#381263
dos2unix : ## Strip the byte-order mark, also known as, BOM, and remove carriage returns
	find \
		./src/ ./test/ \
		\( -name "*.cs" -o -name "*.cshtml" \) \
		-type f \
		-exec sed -i -e "$(shell printf '1s/^\357\273\277//')" -e "s/\r//" {} +
	# find . -type f -exec dos2unix {} \;
.PHONY : dos2unix

# TODO We run `dotnet-format` three times because subsequent runs sometimes
# still make changes. It would be best if we'd check the output to figure out
# whether changes have been made or not and run it again and again until no
# more changes are made.
dosformat : ## Format code and add byte-order marks
	dotnet tool run dotnet-format
	dotnet tool run dotnet-format
	dotnet tool run dotnet-format
.PHONY : dosformat

format : dosformat dos2unix ## Format code
.PHONY : format

assets : ## Build assets
	npm run all
.PHONY : assets

updatepackages : ## Update packages
	cd ./src/ && dotnet list package | awk '/>/ {print $$2}' | xargs -n 1 dotnet add package
	cd ./test/ && dotnet list package | awk '/>/ {print $$2}' | xargs -n 1 dotnet add package
.PHONY : updatepackages

updatetools : ## Update tools
	dotnet tool list | tail -n +3 | awk '{print $$1}' | xargs -n 1 dotnet tool update
	dotnet tool run dotnet-sos install
.PHONY : updatetools

update : updatepackages updatetools ## Update packages and tools
.PHONY : update

# https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-trace-instructions.md#using-dotnet-trace
trace : ## Trace the dotnet process `Icon`
	dotnet tool run \
		dotnet-trace collect \
			--process-id $$(dotnet tool run dotnet-trace ps | grep Icon | awk '{print $$1}') \
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
