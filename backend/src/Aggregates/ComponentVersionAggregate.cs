using Icon;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionAggregate
      : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; set; }

        public ComponentInformationAggregateData Information { get; set; }

#nullable disable
        public ComponentVersionAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentVersionCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.ComponentVersionId;
            ComponentId = data.ComponentId;
            Information = ComponentInformationAggregateData.From(data.Information);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
              return
                Result.Combine(
                    base.Validate(),
                    ValidateEmpty(ComponentId),
                    ValidateNull(Information)
                    );

            else
              return
                Result.Combine(
                    base.Validate(),
                    ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                    Information.Validate()
                    );
        }

        public Result<Models.ComponentVersion, Errors> ToModel()
        {
          var virginResult = ValidateNonVirgin();
          if (virginResult.IsFailure)
            return Result.Failure<ValueObjects.ComponentVersion, Errors>(virginResult.Error);

          var idResult = ValueObjects.Id.From(Id);
          var componentIdResult = ValueObjects.Id.From(componentId);
          var informationResult = Information.ToValueObject();
          var timestampResult = ValueObjects.Timestamp.From(Timestamp);

          var errors = Errors.From(
              idResult,
              componentIdResult,
              informationResult,
              timestampResult
              );

          if (!errors.IsEmpty())
            return Result.Failure<ValueObjects.ComponentVersion, Errors>(errors);

          return Models.ComponentVersion.From(
              id: idResult.Value,
              componentId: componentIdResult.Value,
              information: informationResult.Value,
              timestamp: timestampResult.Value
              );
        }
    }
}