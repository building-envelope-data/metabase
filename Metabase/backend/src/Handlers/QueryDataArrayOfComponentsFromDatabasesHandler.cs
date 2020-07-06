using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Handlers
{
    public abstract class QueryDataArrayOfComponentsFromDatabasesHandler<TDataModel, TGraphQlDataResponse>
        : QueryDataOfComponentsFromDatabasesHandler<
            Queries.QueryDataArrayOfComponentsFromDatabases<TDataModel>,
            IEnumerable<Result<TDataModel, Errors>>,
            IEnumerable<TGraphQlDataResponse>
          >
    {
        public QueryDataArrayOfComponentsFromDatabasesHandler(
            string graphQlQueryName,
            string? graphQlQueryFields,
            IAggregateRepository repository
            )
          : base(
              graphQlQueryName: graphQlQueryName,
              graphQlQueryFields: graphQlQueryFields,
              repository: repository
              )
        {
        }

        protected override Result<IEnumerable<Result<TDataModel, Errors>>, Errors> ParseGraphQlResponse(
            Models.Database database,
            IEnumerable<TGraphQlDataResponse> responses,
            IReadOnlyList<object> path
            )
        {
            return
              Result.Ok<IEnumerable<Result<TDataModel, Errors>>, Errors>(
                  responses.Select(response =>
                    ParseGraphQlResponse(
                      database,
                      response,
                      path
                      )
                    )
                  .ToList().AsReadOnly() // TODO If we don't consume the stream immediately here, it is lost. Why?
                  );
        }

        protected abstract Result<TDataModel, Errors> ParseGraphQlResponse(
            Models.Database database,
            TGraphQlDataResponse response,
            IReadOnlyList<object> path
            );

        protected override
          Result<IEnumerable<Result<TDataModel, Errors>>, Errors>
          MergeDatabaseResponsesForTimestampedId(
            TimestampedId timestampedId,
            IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>> dataResultsResults
            )
        {
            return
              MergeDatabaseResponsesForTimestampedId<TDataModel>(
                  timestampedId,
                  dataResultsResults
                  );
        }
    }
}