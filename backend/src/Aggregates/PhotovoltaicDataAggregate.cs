using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using JToken = Newtonsoft.Json.Linq.JToken;

namespace Icon.Aggregates
{
    public sealed class PhotovoltaicDataAggregate
      : DataAggregate, IConvertible<Models.PhotovoltaicData>
    {
#nullable disable
        public PhotovoltaicDataAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.PhotovoltaicDataCreated> @event)
        {
            ApplyCreated(@event);
        }

        private void Apply(Marten.Events.Event<Events.PhotovoltaicDataDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public Result<Models.PhotovoltaicData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.PhotovoltaicData, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentIdResult = ValueObjects.Id.From(ComponentId);
            var dataResult = ValueObjects.PhotovoltaicDataJson.FromNestedCollections(Data);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.PhotovoltaicData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}