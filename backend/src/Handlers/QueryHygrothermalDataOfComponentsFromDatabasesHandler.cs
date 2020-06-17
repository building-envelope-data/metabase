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
    public sealed class QueryHygrothermalDataOfComponentsFromDatabasesGraphQlResponse
    {
        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public System.Text.Json.JsonElement? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public sealed class QueryHygrothermalDataOfComponentsFromDatabasesHandler
      : QueryDataArrayOfComponentsFromDatabasesHandler<
          Models.HygrothermalDataFromDatabase,
          QueryHygrothermalDataOfComponentsFromDatabasesGraphQlResponse
        >
    {
        public QueryHygrothermalDataOfComponentsFromDatabasesHandler(
                        IAggregateRepository repository
                        )
          : base(
              graphQlQueryName: "hygrothermalData",
              graphQlQueryFields: @"
                id
                componentId
                data
                timestamp
              ",
              repository: repository
              )
        {
        }

        protected override Result<Models.HygrothermalDataFromDatabase, Errors> ParseGraphQlResponse(
            Models.Database database,
            QueryHygrothermalDataOfComponentsFromDatabasesGraphQlResponse hygrothermalDataResponse,
            IReadOnlyList<object> path
            )
        {
            var idResult = ValueObjects.Id.From(hygrothermalDataResponse.Id, path.Append("id").ToList().AsReadOnly());
            var componentIdResult = ValueObjects.Id.From(hygrothermalDataResponse.ComponentId, path.Append("componentId").ToList().AsReadOnly());
            var dataResult =
              hygrothermalDataResponse.Data is null
              ? Result.Failure<ValueObjects.HygrothermalDataJson, Errors>(
                  Errors.One(
                    message: $"The value for key `data` is of the hygrothermal data GraphQL response {hygrothermalDataResponse.Id} with timestamp {hygrothermalDataResponse.Timestamp} for component {hygrothermalDataResponse.ComponentId} from database {database.Id} is `null`",
                    code: ErrorCodes.InvalidValue,
                    path: path.Append("data").ToList().AsReadOnly()
                    )
                  )
              : ValueObjects.HygrothermalDataJson.FromJsonElement(
                  hygrothermalDataResponse.Data ?? throw new ArgumentNullException(nameof(hygrothermalDataResponse.Data)), // TODO Why does the null-forgiving operator `!` not work here?
                  path.Append("data").ToList().AsReadOnly()
                  );
            var timestampResult = ValueObjects.Timestamp.From(hygrothermalDataResponse.Timestamp, path: path.Append("timestamp").ToList().AsReadOnly());
            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.HygrothermalDataFromDatabase.From(
                    id: idResult.Value,
                    databaseId: database.Id,
                    componentId: componentIdResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}