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
            Console.WriteLine(database);
            foreach (var response in responses)
            {
                Console.WriteLine(response);
            }
            return
              Result.Ok<IEnumerable<Result<TDataModel, Errors>>, Errors>(
                  responses.Select(response =>
                    ParseGraphQlResponse(
                      database,
                      response,
                      path
                      )
                    )
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
            ValueObjects.TimestampedId timestampedId,
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