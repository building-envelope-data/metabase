using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using JToken = Newtonsoft.Json.Linq.JToken;

namespace Icon.Aggregates
{
    public sealed class OpticalDataAggregate
      : DataAggregate, IConvertible<Models.OpticalData>
    {
#nullable disable
        public OpticalDataAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.OpticalDataCreated> @event)
        {
            ApplyCreated(@event);
        }

        private void Apply(Marten.Events.Event<Events.OpticalDataDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public Result<Models.OpticalData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.OpticalData, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentIdResult = ValueObjects.Id.From(ComponentId);
            var dataResult = ValueObjects.OpticalDataJson.FromNestedCollections(Data);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

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
                    componentId: componentIdResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}