# Concise introduction to GNU Make:
# https://swcarpentry.github.io/make-novice/reference.html

SHELL := /usr/bin/env bash
.SHELLFLAGS := -o errexit -o errtrace -o nounset -o pipefail -c
MAKEFLAGS += --warn-undefined-variables

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

# List of dotnet commands
# https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

# See https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-run-and-debug?tabs=dotnetcli
list : ARGS =
list : ## List tests, for example, `make list ARGS='--filter "FullyQualifiedName~LoggedInUser_CreatesComponent"'`
	dotnet run \
		--project ./test/*.Tests.csproj \
		--list-tests \
		${ARGS}
.PHONY : list

# See https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-run-and-debug?tabs=dotnetcli
# dotnet test --filter "FullyQualifiedName~${x}" --verbosity minimal
# dotnet retest -- --filter "FullyQualifiedName~${x}" --verbosity minimal
test : ARGS =
test : ## Run (previously failed) tests, possibly with custom arguments `${ARGS}` from `dotnet test --help`, for example, `make test` or `make test ARGS='--filter "FullyQualifiedName~InvalidResetCode_IsUserError"'`
	dotnet run \
		--project ./test/*.Tests.csproj \
		--coverage \
		${ARGS}
.PHONY : test

play : ## Run tests in `Metabase.Tests.Play` (semi-alternative to the buggy `dotnet script` REPL)
	dotnet run \
		--project ./test/*.Tests.csproj \
		--filter "FullyQualifiedName~Metabase.Tests.Play"
.PHONY : play

info : ## Displays advanced information about the .NET Test Application such as ... The platform. The environment. Each registered command line provider, such as its name, version, description, and options. Each registered tool, such as its command, name, version, description, and all command-line providers.xxx
	dotnet run \
		--project ./test/*.Tests.csproj \
		--info
.PHONY : info

# For usage information see https://github.com/tomchavakis/nuget-license#usage
# or run `dotnet tool run dotnet-project-licenses -- --help`
licenses : ## Print licenses
	dotnet tool run \
		dotnet-project-licenses \
			-i ./*.sln
			# --json
			# --allowed-license-types ...
			# --forbidden-license-types ...
.PHONY : licenses

# https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-trace-instructions.md#using-dotnet-trace
trace : ## Trace the dotnet process `Metabase`
	dotnet tool run \
		dotnet-trace collect \
			--process-id "$$(dotnet tool run dotnet-trace ps | grep Metabase | awk '{print $$1}')" \
			--format speedscope \
			--providers Microsoft-Windows-DotNETRuntime
.PHONY : trace

# If `graphqurl` were installed, the schema could be introspected with
# `gq http://backend:8080/graphql/ --introspect`
# See https://github.com/hasura/graphqurl#export-schema
update-schema : ## Update GraphQl schema
	wget \
		--output-document ./test/Integration/GraphQl/__snapshots__/GraphQlSchemaTests.IsUnchanged.snap \
		http://backend:8080/graphql?sdl
.PHONY : update-schema

# https://github.com/JosefPihrt/Roslynator/blob/master/docs/cli/README.md
analyze : ## Analyze with Roslynator
	dotnet tool run roslynator analyze
.PHONY : analyze

# https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html#install-and-use-resharper-command-line-tools-as-net-core-tools
inspect : ## Inspect with ReSharper, for example, `make inspect OUTPUT=./inspection.xml`
	dotnet tool run jb inspectcode --build --output "${OUTPUT}" ./*.sln
.PHONY : inspect

editorconfig : ## Debug editorconfig
	editorconfig ./.editorconfig
.PHONY : editorconfig

# TODO Apply not only to `src` folder but ignore files and directories within .gitignore.
#      With git installed there are various options, which all have some
#      disadvantages, see
#      https://unix.stackexchange.com/questions/358270/find-files-that-are-not-in-gitignore
# Inspired by
# * https://stackoverflow.com/questions/45240387/how-can-i-remove-the-bom-from-a-utf-8-file/45240995#45240995
# * https://unix.stackexchange.com/questions/381230/how-can-i-remove-the-bom-from-a-utf-8-file/381263#381263
dos2unix : ## Strip the byte-order mark, also known as, BOM, and remove carriage returns
	find \
		. \
		\( -name "*.cs" -o -name "*.cshtml" \) \
		-type f \
		-exec sed -i -e "$(shell printf '1s/^\357\273\277//')" -e "s/\r//" {} +
	# find . -type f -exec dos2unix {} \;
.PHONY : dos2unix

# See https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-format
# We do not run JetBrains ReSharper cleanupcode because it does wrong things
# like removing private setters needed by EntityFramework Core. We used to run
# it as follows: For formatting
# `dotnet tool run jb cleanupcode --profile="Built-in: Reformat Code" ./*.sln`
# For syntax styling
# `dotnet tool run jb cleanupcode --profile="Built-in: Reformat & Apply Syntax Style" ./*.sln`
# For cleanup
# `dotnet tool run jb cleanupcode ./*.sln`
# The profile `Built-in: Full Cleanup` does not update file headers.
# See https://www.jetbrains.com/help/resharper/Code_Cleanup__Index.html#profiles
format : ## Format code
	dotnet format ./*.sln
	$(MAKE) dos2unix
.PHONY : format

outdated-packages : ## List outdated packages
	dotnet tool run \
		dotnet-outdated
.PHONY : outdated-packages

update-packages : ## Update packages
	dotnet tool run \
		dotnet-outdated \
		--upgrade
.PHONY : update-packages

update-tools : ## Update tools
	dotnet tool list \
		| tail --lines=+3 \
		| awk '{print $$1}' \
		| xargs --max-args=1 dotnet tool update
	dotnet tool run dotnet-sos install
.PHONY : update-tools

update : ## Update packages and tools
	-$(MAKE) update-packages
	-$(MAKE) update-tools
.PHONY : update

restore : VERBOSITY = normal
restore : ## Restore packages and tools with verbosity level `${VERBOSITY}` that defaults to `normal` (allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]), for example, `make restore VERBOSITY=detailed`
	dotnet restore --verbosity ${VERBOSITY}
.PHONY : restore

dotbuild : ## Build projects
	dotnet build
.PHONY : dotbuild

dedup : ## Dedeuplicate code lines matching the pattern `${PATTERN}`, for example, `make dedup PATTERN="using Infrastructure.Aggregates"`
	find . -name "*.cs" \
		| xargs -n 1 \
				gawk -i inplace "{ \
					if (/${PATTERN}/) \
						{ if (!seen[$$0]++) { print } } \
					else \
						{ print } \
					}"
.PHONY : dedup

check-database : ## Check whether there have been any model changes made since the last migration was added and whether the C# domain model matches the PostgreSQL database schema
	$(MAKE) test ARGS='--filter "FullyQualifiedName~EnsureMigrationsAreUpToDate"'
	$(MAKE) test ARGS='--filter "FullyQualifiedName~EnsureDomainModelMatchesDatabaseSchema"'
.PHONY : check-database

has-pending-model-changes : ## Check whether there have been any model changes made since the last migration was added
	dotnet ef \
		--project ./src/*.csproj \
		--startup-project ./src/*.csproj \
		migrations \
		has-pending-model-changes
.PHONY : has-pending-model-changes

# https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#sql-scripts
migration : ## Add migration with name `${NAME}`, generate corresponding idempotent migration SQL script, and append it to ./src/Migrations/migrate.sql, for example, `make migration NAME=AddStatementToDataApproval`
	old=$$(find ./src/Migrations/ -type f -regex ".*/[^/.]*_[^/.]*\.cs" -exec basename --suffix=.cs {} \; | sort | tail --lines=1 ) && \
	echo old=$$old && \
	dotnet ef \
		--project ./src/*.csproj \
		--startup-project ./src/*.csproj \
		migrations \
		add \
		${NAME} && \
	new=$$(find ./src/Migrations/ -type f -regex ".*/[^/.]*_[^/.]*\.cs" -exec basename --suffix=.cs {} \; | sort | tail --lines=1 ) && \
	echo new=$$new && \
	migrate_path=./src/Migrations/migrate_from_$${old}_to_$${new}.sql && \
	rollback_path=./src/Migrations/rollback_from_$${new}_to_$${old}.sql && \
	dotnet ef \
		--project ./src/*.csproj \
		--startup-project ./src/*.csproj \
		migrations \
		script \
		--idempotent \
		$${old} \
		$${new} \
		--output $${migrate_path} && \
	dotnet ef \
		--project ./src/*.csproj \
		--startup-project ./src/*.csproj \
		migrations \
		script \
		--idempotent \
		$${new} \
		$${old} \
		--output $${rollback_path} && \
	sed -i -e "$(shell printf '1s/^\357\273\277//')" -e "s/\r//" $${migrate_path} $${rollback_path} && \
	cat $${migrate_path} >> ./src/Migrations/migrate.sql && \
	$(MAKE) dos2unix && \
	echo "Review the generated C# and SQL files and make necessary adjustments consistently across all the files. In particular, keep $${migrate_path} and ./src/Migrations/migrate.sql in sync."
.PHONY : migration
