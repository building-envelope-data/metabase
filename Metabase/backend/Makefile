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
