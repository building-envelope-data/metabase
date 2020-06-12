using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using JToken = Newtonsoft.Json.Linq.JToken;

namespace Icon.Aggregates
{
    public sealed class CalorimetricDataAggregate
      : DataAggregate, IConvertible<Models.CalorimetricData>
    {
#nullable disable
        public CalorimetricDataAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.CalorimetricDataCreated> @event)
        {
            ApplyCreated(@event);
        }

        private void Apply(Marten.Events.Event<Events.CalorimetricDataDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public Result<Models.CalorimetricData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.CalorimetricData, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentIdResult = ValueObjects.Id.From(ComponentId);
            var dataResult = ValueObjects.CalorimetricDataJson.FromNestedCollections(Data);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.CalorimetricData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}