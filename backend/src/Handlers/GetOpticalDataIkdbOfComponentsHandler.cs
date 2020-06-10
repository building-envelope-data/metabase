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
        public sealed class OpticalDataGraphQlResponse
        {
            public Guid Id { get; set; }
            public Guid ComponentId { get; set; }
            public System.Text.Json.JsonElement? Data { get; set; }
            public DateTime Timestamp { get; set; }
        }

    public sealed class GetOpticalDataIkdbOfComponentsHandler
      : QueryDatabasesHandler<
          Queries.GetOpticalDataIkdbOfComponents,
          IEnumerable<Result<IEnumerable<Result<Models.OpticalDataIkdb, Errors>>, Errors>>,
          IDictionary<string, IEnumerable<OpticalDataGraphQlResponse>>
        >
    {
        public GetOpticalDataIkdbOfComponentsHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override GraphQLRequest CreateGraphQlRequest(
            Queries.GetOpticalDataIkdbOfComponents query
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
                          $@"opticalData{index}: opticalData(
                            componentId: ""{(Guid)timestampedId.Id}"",
                            timestamp: ""{timestampedId.Timestamp.InUtcFormat()}""
                          ) {{
                            id
                            componentId
                            data
                            timestamp
                          }}"
                        )
                      )
                  }
                }}"
            };
        }

        protected override IEnumerable<Result<IEnumerable<Result<Models.OpticalDataIkdb, Errors>>, Errors>> ParseGraphQlResponse(
            Models.Database database,
            IDictionary<string, IEnumerable<OpticalDataGraphQlResponse>> response
            )
        {
                return
                  response.Select(pair =>
                    Result.Ok<IEnumerable<Result<Models.OpticalDataIkdb, Errors>>, Errors>(
                      pair.Value.Select(opticalDataResponse =>
                        ParseOpticalDataResponse(database, opticalDataResponse, new List<object>().AsReadOnly())
                        )
                      )
                    );
        }

        private Result<Models.OpticalDataIkdb, Errors> ParseOpticalDataResponse(
            Models.Database database,
            OpticalDataGraphQlResponse opticalDataResponse,
            IReadOnlyList<object> path
            )
        {
          var idResult = ValueObjects.Id.From(opticalDataResponse.Id, path.Append("id").ToList().AsReadOnly());
          var componentIdResult = ValueObjects.Id.From(opticalDataResponse.ComponentId, path.Append("componentId").ToList().AsReadOnly());
          var dataResult =
            opticalDataResponse.Data is null
            ? Result.Failure<ValueObjects.OpticalDataJson, Errors>(
                Errors.One(
                  message: $"The value for key `data` is of the optical data GraphQL response {opticalDataResponse.Id} with timestamp {opticalDataResponse.Timestamp} for component {opticalDataResponse.ComponentId} from database {database.Id} is `null`",
                  code: ErrorCodes.InvalidValue,
                  path: path.Append("data").ToList().AsReadOnly()
                  )
                )
            : ValueObjects.OpticalDataJson.FromJsonElement(
                opticalDataResponse.Data ?? throw new ArgumentNullException(nameof(opticalDataResponse.Data)), // TODO Why does the null-forgiving operator `!` not work here?
                path.Append("data").ToList().AsReadOnly()
                );
          var timestampResult = ValueObjects.Timestamp.From(opticalDataResponse.Timestamp, path: path.Append("timestamp").ToList().AsReadOnly());
          return
            Errors.Combine(
                idResult,
                componentIdResult,
                dataResult,
                timestampResult
                )
            .Bind(_ =>
                Models.OpticalDataIkdb.From(
                  id: idResult.Value,
                  databaseId: database.Id,
                  componentId: componentIdResult.Value,
                  data: dataResult.Value,
                  timestamp: timestampResult.Value
                  )
                );
        }

        protected override IEnumerable<Result<IEnumerable<Result<Models.OpticalDataIkdb, Errors>>, Errors>> MergeDatabaseResponses(
            Queries.GetOpticalDataIkdbOfComponents query,
            IEnumerable<Result<IEnumerable<Result<IEnumerable<Result<Models.OpticalDataIkdb, Errors>>, Errors>>, Errors>> responseResults
            )
        {
            // TODO Remove to ----
            foreach (var result in responseResults)
            {
                if (result.IsFailure)
                {
                  Console.WriteLine("#####");
                  Console.WriteLine(result.Error);
                }
            }
            // ----
            var responseResultEnumerators =
              responseResults
              .Where(result => result.IsSuccess) // TODO Report failed results (and which databases).
              .Select(result => result.Value)
              .Select(dataResults => dataResults.GetEnumerator());
            // TODO Remove to ----
            Func<Errors, IEnumerable<Result<Models.OpticalDataIkdb, Errors>>> xxx =
              a =>
            {
              Console.WriteLine("@@@@@");
              Console.WriteLine(a);
              return Enumerable.Empty<Result<Models.OpticalDataIkdb, Errors>>();
            };
            // ----
            return
              query.TimestampedIds
              .Select(_ =>
                  Result.Ok<IEnumerable<Result<Models.OpticalDataIkdb, Errors>>, Errors>(
                    responseResultEnumerators.SelectMany(dataResultEnumerator =>
                      // TODO Remove to ----
                      dataResultEnumerator.MoveNext()
                      ? (dataResultEnumerator.Current.IsSuccess ? dataResultEnumerator.Current.Value : xxx(dataResultEnumerator.Current.Error) )
                      : Enumerable.Empty<Result<Models.OpticalDataIkdb, Errors>>()
                      // ----
                      /* dataResultEnumerator.MoveNext() && dataResultEnumerator.Current.IsSuccess // TODO Report failed data results somehow. */
                      /* ? dataResultEnumerator.Current.Value */
                      /* : Enumerable.Empty<Result<Models.OpticalDataIkdb, Errors>>() */
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
