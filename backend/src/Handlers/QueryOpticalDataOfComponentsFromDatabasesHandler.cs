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
    public sealed class QueryOpticalDataOfComponentsFromDatabasesGraphQlResponse
    {
        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public System.Text.Json.JsonElement? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public sealed class QueryOpticalDataOfComponentsFromDatabasesHandler
      : QueryDataArrayOfComponentsFromDatabasesHandler<
          Models.OpticalDataFromDatabase,
          QueryOpticalDataOfComponentsFromDatabasesGraphQlResponse
        >
    {
        public QueryOpticalDataOfComponentsFromDatabasesHandler(
                        IAggregateRepository repository
                        )
          : base(
              graphQlQueryName: "opticalData",
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

        protected override Result<Models.OpticalDataFromDatabase, Errors> ParseGraphQlResponse(
            Models.Database database,
            QueryOpticalDataOfComponentsFromDatabasesGraphQlResponse opticalDataResponse,
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
                  Models.OpticalDataFromDatabase.From(
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