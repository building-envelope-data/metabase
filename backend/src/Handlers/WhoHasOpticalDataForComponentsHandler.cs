using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using Marten;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using Exception = System.Exception;
using Guid = System.Guid;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using Queries = Icon.Queries;
using GraphQLRequest = GraphQL.GraphQLRequest;
using GraphQL.Client.Http; // AsGraphQLHttpResponse

namespace Icon.Handlers
{
    public sealed class WhoHasOpticalDataForComponentsHandler
      : QueryDatabasesHandler<
          Queries.WhoHasOpticalDataForComponents,
          IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>,
          IDictionary<string, bool>
        >
    {
        public WhoHasOpticalDataForComponentsHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override GraphQLRequest CreateGraphQlRequest(
            Queries.WhoHasOpticalDataForComponents query
            )
        {
            return new GraphQLRequest
            {
              Query =
                // For interpolated strings `$"..."`, see
                // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
                // For verbatim strings `@"..."`, see
                // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim
                $@"query {{
                  {
                    string.Join(
                        "\n",
                        query.TimestampedIds.Select((timestampedId, index) =>
                          $@"hasOpticalData{index}: hasOpticalData(
                            componentId: ""{(Guid)timestampedId.Id}"",
                            timestamp: ""{timestampedId.Timestamp.InUtcFormat()}""
                          )"
                        )
                      )
                  }
                }}"
            };
        }

        protected override IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>> ParseGraphQlResponse(
            Models.Database database,
            IDictionary<string, bool> response
            )
        {
            return
              response.Select(pair =>
                  Result.Ok<IEnumerable<Result<Models.Database, Errors>>, Errors>(
                    pair.Value
                    ? new Result<Models.Database, Errors>[]
                    {
                      Result.Ok<Models.Database, Errors>(database)
                    }
                    : Enumerable.Empty<Result<Models.Database, Errors>>()
                    )
                  );
        }

        protected override
          IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>
          MergeDatabaseResponses(
            Queries.WhoHasOpticalDataForComponents query,
            IEnumerable<Result<IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>, Errors>> responseResults
            )
        {
            var responseResultEnumerators =
              responseResults
              .Where(result => result.IsSuccess) // TODO Report failed results (and which databases).
              .Select(result => result.Value.GetEnumerator());
            return
              query.TimestampedIds
              .Select(_ =>
                  Result.Ok<IEnumerable<Result<Models.Database, Errors>>, Errors>(
                    responseResultEnumerators.SelectMany(dataResultEnumerator =>
                      dataResultEnumerator.MoveNext() && dataResultEnumerator.Current.IsSuccess // TODO Report failed data results somehow.
                      ? dataResultEnumerator.Current.Value
                      : Enumerable.Empty<Result<Models.Database, Errors>>()
                      )
                    .ToList().AsReadOnly()
                    )
                  )
              .ToList().AsReadOnly();
            // We turn the enumerable into a list so that consuming it
            // more than once does not consume the enumerators more than
            // once, which would only be possible if we called `Reset` on
            // them after `MoveNext` returned `false`, see
            // https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator?view=netcore-3.1#remarks
        }
    }
}
