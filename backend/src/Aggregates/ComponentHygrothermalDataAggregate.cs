using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentHygrothermalDataAggregate
      : EventSourcedAggregate, IOneToManyAssociationAggregate, IConvertible<Models.ComponentHygrothermalData>
    {
        // The instance variables `ComponentId` and `HygrothermalDataId` are
        // abbreviated to make the corresponding database index have less than
        // 64 characters (which is Postgres maximum). I would prefer not
        // abbreviating the variable names but specifying custom database
        // index names instead.
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; set; }

        [ForeignKey(typeof(HygrothermalDataAggregate))]
        public Guid HygrothermalDataId { get; set; }

        public Guid ParentId { get => ComponentId; }
        public Guid AssociateId { get => HygrothermalDataId; }

#nullable disable
        public ComponentHygrothermalDataAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentHygrothermalDataAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            ComponentId = data.ComponentId;
            HygrothermalDataId = data.HygrothermalDataId;
        }

        private void Apply(Marten.Events.Event<Events.ComponentHygrothermalDataRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(ComponentId, nameof(ComponentId)),
                    ValidateEmpty(HygrothermalDataId, nameof(HygrothermalDataId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  ValidateNonEmpty(HygrothermalDataId, nameof(HygrothermalDataId))
                  );
        }

        public Result<Models.ComponentHygrothermalData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentHygrothermalData, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentIdResult = ValueObjects.Id.From(ComponentId);
            var opticalDataIdResult = ValueObjects.Id.From(HygrothermalDataId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  opticalDataIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentHygrothermalData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    opticalDataId: opticalDataIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}