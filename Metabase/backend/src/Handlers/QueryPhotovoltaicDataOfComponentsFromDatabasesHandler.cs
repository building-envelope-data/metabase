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
    public sealed class QueryPhotovoltaicDataOfComponentsFromDatabasesGraphQlResponse
    {
        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public System.Text.Json.JsonElement? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public sealed class QueryPhotovoltaicDataOfComponentsFromDatabasesHandler
      : QueryDataArrayOfComponentsFromDatabasesHandler<
          Models.PhotovoltaicDataFromDatabase,
          QueryPhotovoltaicDataOfComponentsFromDatabasesGraphQlResponse
        >
    {
        public QueryPhotovoltaicDataOfComponentsFromDatabasesHandler(
                        IAggregateRepository repository
                        )
          : base(
              graphQlQueryName: "photovoltaicData",
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

        protected override Result<Models.PhotovoltaicDataFromDatabase, Errors> ParseGraphQlResponse(
            Models.Database database,
            QueryPhotovoltaicDataOfComponentsFromDatabasesGraphQlResponse photovoltaicDataResponse,
            IReadOnlyList<object> path
            )
        {
            var idResult = Infrastructure.ValueObjects.Id.From(photovoltaicDataResponse.Id, path.Append("id").ToList().AsReadOnly());
            var componentIdResult = Infrastructure.ValueObjects.Id.From(photovoltaicDataResponse.ComponentId, path.Append("componentId").ToList().AsReadOnly());
            var dataResult =
              photovoltaicDataResponse.Data is null
              ? Result.Failure<PhotovoltaicDataJson, Errors>(
                  Errors.One(
                    message: $"The value for key `data` is of the photovoltaic data GraphQL response {photovoltaicDataResponse.Id} with timestamp {photovoltaicDataResponse.Timestamp} for component {photovoltaicDataResponse.ComponentId} from database {database.Id} is `null`",
                    code: ErrorCodes.InvalidValue,
                    path: path.Append("data").ToList().AsReadOnly()
                    )
                  )
              : PhotovoltaicDataJson.FromJsonElement(
                  photovoltaicDataResponse.Data ?? throw new ArgumentNullException(nameof(photovoltaicDataResponse.Data)), // TODO Why does the null-forgiving operator `!` not work here?
                  path.Append("data").ToList().AsReadOnly()
                  );
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(photovoltaicDataResponse.Timestamp, path: path.Append("timestamp").ToList().AsReadOnly());
            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.PhotovoltaicDataFromDatabase.From(
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