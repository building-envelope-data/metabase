using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregates;

namespace Icon.Handlers
{
    public sealed class WhichDatabasesHaveDataForComponentsHandler<TDataModel>
      : QueryDataOfComponentsFromDatabasesHandler<
          Queries.WhichDatabasesHaveDataForComponents<TDataModel>,
          IEnumerable<Result<Models.Database, Errors>>,
          bool
        >
    {
        public WhichDatabasesHaveDataForComponentsHandler(
                        string graphQlQueryName,
                        IAggregateRepository repository
                        )
          : base(
                            graphQlQueryName: graphQlQueryName,
                            graphQlQueryFields: null,
                            repository: repository
                            )
        {
        }

        protected override Result<IEnumerable<Result<Models.Database, Errors>>, Errors> ParseGraphQlResponse(
            Models.Database database,
            bool response,
            IReadOnlyList<object> path
            )
        {
            return
                            Result.Ok<IEnumerable<Result<Models.Database, Errors>>, Errors>(
                                    response
                                    ? new Result<Models.Database, Errors>[]
                                    {
                                    Result.Ok<Models.Database, Errors>(database)
                                    }
                                    : Enumerable.Empty<Result<Models.Database, Errors>>()
                                    );
        }

        protected override
                    Result<IEnumerable<Result<Models.Database, Errors>>, Errors>
                    MergeDatabaseResponsesForTimestampedId(
            ValueObjects.TimestampedId timestampedId,
            IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>> dataResultsResults
            )
        {
            return
              MergeDatabaseResponsesForTimestampedId<Models.Database>(
                  timestampedId,
                  dataResultsResults
                  );
        }
    }
}