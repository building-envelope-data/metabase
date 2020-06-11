using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using GraphQL.Client.Http; // AsGraphQLHttpResponse
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using Marten;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using Exception = System.Exception;
using GraphQLRequest = GraphQL.GraphQLRequest;
using Guid = System.Guid;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using Queries = Icon.Queries;

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