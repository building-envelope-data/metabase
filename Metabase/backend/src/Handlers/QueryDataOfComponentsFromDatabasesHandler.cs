using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;
using GraphQLRequest = GraphQL.GraphQLRequest;
using Guid = System.Guid;

namespace Metabase.Handlers
{
    public abstract class QueryDataOfComponentsFromDatabasesHandler<TQuery, TData, TGraphQlResponse>
        : QueryDatabasesHandler<
            TQuery,
            IEnumerable<Result<TData, Errors>>,
            IDictionary<string, TGraphQlResponse>
          >
      where TQuery : Queries.QueryDataOfComponentsFromDatabases<TData>
    {
        private readonly string _graphQlQueryName;
        private readonly string? _graphQlQueryFields;

        protected QueryDataOfComponentsFromDatabasesHandler(
            string graphQlQueryName,
            string? graphQlQueryFields,
            IAggregateRepository repository
            )
          : base(repository)
        {
            _graphQlQueryName = graphQlQueryName;
            _graphQlQueryFields = graphQlQueryFields;
        }

        protected override GraphQLRequest CreateGraphQlRequest(
            TQuery query
            )
        {
            return new GraphQLRequest
            {
                Query =
                  $@"query {{
                    {
                      string.Join(
                          "\n",
                          query.TimestampedIds.Select((timestampedId, index) =>
                            $@"{_graphQlQueryName}{index}: {_graphQlQueryName}(
                              componentId: ""{(Guid)timestampedId.Id}"",
                              timestamp: ""{timestampedId.Timestamp.InUtcFormat()}""
                            )"
                            +
                            (_graphQlQueryFields is null ? "" : $"{{{_graphQlQueryFields}}}")
                          )
                        )
                    }
                  }}"
                // For interpolated strings `$"..."`, see
                // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
                // For verbatim strings `@"..."`, see
                // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim
            };
        }

        protected override IEnumerable<Result<TData, Errors>> ParseGraphQlResponse(
            Models.Database database,
            IDictionary<string, TGraphQlResponse> responses
            )
        {
            return
              responses.Select(pair =>
                  ParseGraphQlResponse(
                    database,
                    pair.Value,
                    new List<object>().AsReadOnly()
                    )
                )
              .ToList().AsReadOnly(); // TODO If we don't consume the stream immediately here, it is lost. Why?
        }

        protected abstract Result<TData, Errors> ParseGraphQlResponse(
            Models.Database database,
            TGraphQlResponse response,
            IReadOnlyList<object> path
            );

        protected override
          IEnumerable<Result<TData, Errors>>
          MergeDatabaseResponses(
            TQuery query,
            IEnumerable<Result<IEnumerable<Result<TData, Errors>>, Errors>> dataResultsResults
            )
        {
            var dataResultsEnumeratorResults =
              dataResultsResults.Select(dataResultsResult =>
                  dataResultsResult.Map(dataResults =>
                    dataResults.GetEnumerator()
                    )
                  )
              .ToList().AsReadOnly(); // TODO If we don't consume the stream immediately here, it is lost. Why?
            return
              query.TimestampedIds
              .Select(timestampedId =>
                  MergeDatabaseResponsesForTimestampedId(
                    timestampedId,
                    dataResultsEnumeratorResults.Select(dataResultsEnumeratorResult =>
                      dataResultsEnumeratorResult.Bind(dataResultsEnumerator =>
                        dataResultsEnumerator.MoveNext()
                        ? dataResultsEnumerator.Current
                        : Result.Failure<TData, Errors>(
                          Errors.One(
                            message: $"Missing data for component {timestampedId.Id} at {timestampedId.Timestamp}",
                            code: ErrorCodes.MissingData
                            // TODO path: ...
                            )
                          )
                        )
                      )
                    .ToList().AsReadOnly() // TODO If we don't consume the stream immediately here, it is lost. Why?
                    )
                  )
              .ToList().AsReadOnly(); // TODO If we don't consume the stream immediately here, it is lost. Why?
            // We turn the enumerables into lists so that consuming them
            // more than once does not consume the enumerators more than
            // once, which would only be possible if we called `Reset` on
            // them after `MoveNext` returned `false`, see
            // https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator?view=netcore-3.1#remarks
        }

        protected abstract Result<TData, Errors> MergeDatabaseResponsesForTimestampedId(
            TimestampedId timestampedId,
            IEnumerable<Result<TData, Errors>> dataResults
            );

        protected
          Result<IEnumerable<Result<TDataModel, Errors>>, Errors>
          MergeDatabaseResponsesForTimestampedId<TDataModel>(
            TimestampedId timestampedId,
            IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>> dataResultsResults
            )
        {
            return
              Result.Ok<IEnumerable<Result<TDataModel, Errors>>, Errors>(
                  dataResultsResults.SelectMany(dataResultsResult =>
                    dataResultsResult.IsSuccess
                    ? dataResultsResult.Value
                    : new Result<TDataModel, Errors>[]
                    {
                    Result.Failure<TDataModel, Errors>(dataResultsResult.Error)
                    }
                    )
                  .ToList().AsReadOnly() // TODO If we don't consume the stream immediately here, it is lost. Why?
                  );
        }
    }
}