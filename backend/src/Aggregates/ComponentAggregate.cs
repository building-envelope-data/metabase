// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using CSharpFunctionalExtensions;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentAggregate
      : EventSourcedAggregate, IConvertible<Models.Component>
    {
        public ComponentInformationAggregateData Information { get; set; }

#nullable disable
        public ComponentAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.ComponentId;
            Information = ComponentInformationAggregateData.From(data.Information);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateNull(Information, nameof(Information))
                    );

            return Result.Combine(
                base.Validate(),
                ValidateNonNull(Information, nameof(Information))
                .Bind(_ => Information.Validate())
                );
        }

        public Result<Models.Component, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.Component, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var informationResult = Information.ToValueObject();
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  informationResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.Component.From(
                    id: idResult.Value,
                    information: informationResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}