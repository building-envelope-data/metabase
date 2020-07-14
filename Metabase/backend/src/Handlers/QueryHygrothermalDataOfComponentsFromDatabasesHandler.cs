using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using DateTime = System.DateTime;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Metabase.Handlers
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
          Models.HygrothermalData,
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

        protected override Result<Models.HygrothermalData, Errors> ParseGraphQlResponse(
            Models.Database database,
            QueryHygrothermalDataOfComponentsFromDatabasesGraphQlResponse hygrothermalDataResponse,
            IReadOnlyList<object> path
            )
        {
            var idResult = Infrastructure.ValueObjects.Id.From(hygrothermalDataResponse.Id, path.Append("id").ToList().AsReadOnly());
            var componentIdResult = Infrastructure.ValueObjects.Id.From(hygrothermalDataResponse.ComponentId, path.Append("componentId").ToList().AsReadOnly());
            var dataResult =
              hygrothermalDataResponse.Data is null
              ? Result.Failure<HygrothermalDataJson, Errors>(
                  Errors.One(
                    message: $"The value for key `data` is of the hygrothermal data GraphQL response {hygrothermalDataResponse.Id} with timestamp {hygrothermalDataResponse.Timestamp} for component {hygrothermalDataResponse.ComponentId} from database {database.Id} is `null`",
                    code: ErrorCodes.InvalidValue,
                    path: path.Append("data").ToList().AsReadOnly()
                    )
                  )
              : HygrothermalDataJson.FromJsonElement(
                  hygrothermalDataResponse.Data ?? throw new ArgumentNullException(nameof(hygrothermalDataResponse.Data)), // TODO Why does the null-forgiving operator `!` not work here?
                  path.Append("data").ToList().AsReadOnly()
                  );
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(hygrothermalDataResponse.Timestamp, path: path.Append("timestamp").ToList().AsReadOnly());
            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.HygrothermalData.From(
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