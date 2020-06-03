using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentOpticalDataAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.ComponentOpticalData>
    {
        // The instance variables `ComponentId` and `OpticalDataId` are
        // abbreviated to make the corresponding database index have less than
        // 64 characters (which is Postgres maximum). I would prefer not
        // abbreviating the variable names but specifying custom database
        // index names instead.
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; set; }

        [ForeignKey(typeof(OpticalDataAggregate))]
        public Guid OpticalDataId { get; set; }

        public Guid ParentId { get => ComponentId; }
        public Guid AssociateId { get => OpticalDataId; }

#nullable disable
        public ComponentOpticalDataAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentOpticalDataAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            ComponentId = data.ComponentId;
            OpticalDataId = data.OpticalDataId;
        }

        private void Apply(Marten.Events.Event<Events.ComponentOpticalDataRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(ComponentId, nameof(ComponentId)),
                    ValidateEmpty(OpticalDataId, nameof(OpticalDataId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  ValidateNonEmpty(OpticalDataId, nameof(OpticalDataId))
                  );
        }

        public Result<Models.ComponentOpticalData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentOpticalData, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentIdResult = ValueObjects.Id.From(ComponentId);
            var opticalDataIdResult = ValueObjects.Id.From(OpticalDataId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  opticalDataIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentOpticalData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    opticalDataId: opticalDataIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}