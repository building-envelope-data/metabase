using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using JToken = Newtonsoft.Json.Linq.JToken;

namespace Icon.Aggregates
{
    public sealed class HygrothermalDataAggregate
      : EventSourcedAggregate, IConvertible<Models.HygrothermalData>
    {
        public object? Data { get; set; }

#nullable disable
        public HygrothermalDataAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.HygrothermalDataCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            Data = data.Data;
        }

        private void Apply(Marten.Events.Event<Events.HygrothermalDataDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateNull(Data, nameof(Data))
                    );

            return Result.Combine(
                base.Validate(),
                ValidateNonNull(Data, nameof(Data))
                );
        }

        public Result<Models.HygrothermalData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.HygrothermalData, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var dataResult = ValueObjects.HygrothermalDataJson.FromNestedCollections(Data);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.HygrothermalData.From(
                    id: idResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}