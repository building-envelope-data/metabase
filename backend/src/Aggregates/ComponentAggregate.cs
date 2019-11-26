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
      : EventSourcedAggregate
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
              return base.Validate();

            else
              return
                Result.Combine(
                    base.Validate(),
                    Information.Validate()
                    );
        }

        public Result<Models.Component, Errors> ToModel()
        {
          var virginResult = ValidateNonVirgin();
          if (virginResult.IsFailure)
            return Result.Failure<ValueObjects.Component, Errors>(virginResult.Error);

          var idResult = ValueObjects.Id.From(Id);
          var informationResult = Information.ToValueObject();
          var timestampResult = ValueObjects.Timestamp.From(Timestamp);

          var errors = Errors.From(
              idResult,
              informationResult,
              timestampResult
              );

          if (!errors.IsEmpty())
            return Result.Failure<ValueObjects.ComponentInformation, Errors>(errors);

          return Models.Component.From(
                    id: idResult.Value,
                    information: informationResult.Value,
                    timestamp: timestampResult.Value
                    );
        }
    }
}