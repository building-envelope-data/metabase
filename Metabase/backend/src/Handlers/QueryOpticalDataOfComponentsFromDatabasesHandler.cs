using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using DateTime = System.DateTime;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Metabase.Handlers
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
          Models.OpticalData,
          QueryOpticalDataOfComponentsFromDatabasesGraphQlResponse
        >
    {
        public QueryOpticalDataOfComponentsFromDatabasesHandler(
                        IModelRepository repository
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

        protected override Result<Models.OpticalData, Errors> ParseGraphQlResponse(
            Models.Database database,
            QueryOpticalDataOfComponentsFromDatabasesGraphQlResponse opticalDataResponse,
            IReadOnlyList<object> path
            )
        {
            var idResult = Infrastructure.ValueObjects.Id.From(opticalDataResponse.Id, path.Append("id").ToList().AsReadOnly());
            var componentIdResult = Infrastructure.ValueObjects.Id.From(opticalDataResponse.ComponentId, path.Append("componentId").ToList().AsReadOnly());
            var dataResult =
              opticalDataResponse.Data is null
              ? Result.Failure<OpticalDataJson, Errors>(
                  Errors.One(
                    message: $"The value for key `data` is of the optical data GraphQL response {opticalDataResponse.Id} with timestamp {opticalDataResponse.Timestamp} for component {opticalDataResponse.ComponentId} from database {database.Id} is `null`",
                    code: ErrorCodes.InvalidValue,
                    path: path.Append("data").ToList().AsReadOnly()
                    )
                  )
              : OpticalDataJson.FromJsonElement(
                  opticalDataResponse.Data ?? throw new ArgumentNullException(nameof(opticalDataResponse.Data)), // TODO Why does the null-forgiving operator `!` not work here?
                  path.Append("data").ToList().AsReadOnly()
                  );
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(opticalDataResponse.Timestamp, path: path.Append("timestamp").ToList().AsReadOnly());
            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.OpticalData.From(
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