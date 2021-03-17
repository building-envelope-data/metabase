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

# List of dotnet commands
# https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet
# TODO Shortcuts for `dotnet aspnet-codegenerator`?

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

update-schema : ## Update GraphQl schema
	-dotnet test --filter "FullyQualifiedName~GraphQlSchemaTests"
	cp test/Integration/GraphQl/__snapshots__/__mismatch__/GraphQlSchemaTests.IsUnchanged.snap test/Integration/GraphQl/__snapshots__/
	cp test/Integration/GraphQl/__snapshots__/__mismatch__/GraphQlSchemaTests.IsUnchanged.snap ../frontend/type-defs.graphqls
.PHONY : update-schema

# https://github.com/JosefPihrt/Roslynator/blob/master/docs/cli/README.md
analyze : ## Analyze with Roslynator
	dotnet tool run roslynator analyze
.PHONY : analyze

play : ## Run facts in `Test.Play` (semi-alternative to the buggy `dotnet script` REPL)
	dotnet test --filter "FullyQualifiedName~Test.Play"
.PHONY : play

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

# TODO We run `dotnet-format` three times because subsequent runs sometimes
# still make changes. It would be best if we'd check the output to figure out
# whether changes have been made or not and run it again and again until no
# more changes are made.
dosformat : ## Format code and add byte-order marks
	dotnet tool run dotnet-format && \
	dotnet tool run dotnet-format && \
	dotnet tool run dotnet-format
.PHONY : dosformat

format : dosformat dos2unix ## Format code
.PHONY : format

update-packages : ## Update packages
	for subproject in src test ; do ( \
		cd ./$${subproject} && \
		dotnet list package | \
			awk '/>/ {print $$2}' | \
			xargs -n 1 dotnet add package \
	) ; done
.PHONY : update-packages

update-tools : ## Update tools
	dotnet tool list | \
		tail -n +3 | \
		awk '{print $$1}' | \
		xargs -n 1 dotnet tool update
	dotnet tool run dotnet-sos install
.PHONY : update-tools

update : update-packages update-tools ## Update packages and tools
.PHONY : update

restore : VERBOSITY = normal
restore : ## Restore packages and tools with verbosity level `${VERBOSITY}` that defaults to `normal` (allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]), for example, `make VERBOSITY=detailed restore`
	dotnet restore --verbosity ${VERBOSITY}
.PHONY : restore

dotbuild : ## Build projects
	dotnet build
.PHONY : dotbuild

dedup : ## Dedeuplicate code lines matching the pattern `${PATTERN}`, for example, `make PATTERN="using Infrastructure.Aggregates" dedup`
	find . -name "*.cs" \
		| xargs -n 1 \
				gawk -i inplace "{ \
					if (/${PATTERN}/) \
						{ if (!seen[$$0]++) { print } } \
					else \
						{ print } \
					}"
.PHONY : dedup

diagrams : ## Draw images from textual UML diagrams
	plantuml diagrams/*.puml
.PHONY : diagrams

# See https://github.com/hasura/graphqurl#export-schema
schema : ## Fetch GraphQL schema using `hasura/graphqurl`
	gq http://backend:8080/graphql/ --introspect > schema.graphql
.PHONY : schema
