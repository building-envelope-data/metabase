using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentPhotovoltaicDataAggregate
      : EventSourcedAggregate, IOneToManyAssociationAggregate, IConvertible<Models.ComponentPhotovoltaicData>
    {
        // The instance variables `ComponentId` and `PhotovoltaicDataId` are
        // abbreviated to make the corresponding database index have less than
        // 64 characters (which is Postgres maximum). I would prefer not
        // abbreviating the variable names but specifying custom database
        // index names instead.
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; set; }

        [ForeignKey(typeof(PhotovoltaicDataAggregate))]
        public Guid PhotovoltaicDataId { get; set; }

        public Guid ParentId { get => ComponentId; }
        public Guid AssociateId { get => PhotovoltaicDataId; }

#nullable disable
        public ComponentPhotovoltaicDataAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentPhotovoltaicDataAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            ComponentId = data.ComponentId;
            PhotovoltaicDataId = data.PhotovoltaicDataId;
        }

        private void Apply(Marten.Events.Event<Events.ComponentPhotovoltaicDataRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(ComponentId, nameof(ComponentId)),
                    ValidateEmpty(PhotovoltaicDataId, nameof(PhotovoltaicDataId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  ValidateNonEmpty(PhotovoltaicDataId, nameof(PhotovoltaicDataId))
                  );
        }

        public Result<Models.ComponentPhotovoltaicData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentPhotovoltaicData, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentIdResult = ValueObjects.Id.From(ComponentId);
            var opticalDataIdResult = ValueObjects.Id.From(PhotovoltaicDataId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  opticalDataIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentPhotovoltaicData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    opticalDataId: opticalDataIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}