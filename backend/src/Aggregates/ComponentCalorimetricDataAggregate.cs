using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentCalorimetricDataAggregate
      : EventSourcedAggregate, IOneToManyAssociationAggregate, IConvertible<Models.ComponentCalorimetricData>
    {
        // The instance variables `ComponentId` and `CalorimetricDataId` are
        // abbreviated to make the corresponding database index have less than
        // 64 characters (which is Postgres maximum). I would prefer not
        // abbreviating the variable names but specifying custom database
        // index names instead.
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; set; }

        [ForeignKey(typeof(CalorimetricDataAggregate))]
        public Guid CalorimetricDataId { get; set; }

        public Guid ParentId { get => ComponentId; }
        public Guid AssociateId { get => CalorimetricDataId; }

#nullable disable
        public ComponentCalorimetricDataAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentCalorimetricDataAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            ComponentId = data.ComponentId;
            CalorimetricDataId = data.CalorimetricDataId;
        }

        private void Apply(Marten.Events.Event<Events.ComponentCalorimetricDataRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(ComponentId, nameof(ComponentId)),
                    ValidateEmpty(CalorimetricDataId, nameof(CalorimetricDataId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  ValidateNonEmpty(CalorimetricDataId, nameof(CalorimetricDataId))
                  );
        }

        public Result<Models.ComponentCalorimetricData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentCalorimetricData, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentIdResult = ValueObjects.Id.From(ComponentId);
            var opticalDataIdResult = ValueObjects.Id.From(CalorimetricDataId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  opticalDataIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentCalorimetricData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    opticalDataId: opticalDataIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}