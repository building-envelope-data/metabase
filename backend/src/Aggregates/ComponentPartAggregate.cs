using CSharpFunctionalExtensions;
using Marten.Schema;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentPartAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.ComponentPart>
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid AssembledComponentId { get; set; }

        [ForeignKey(typeof(ComponentAggregate))]
        public Guid PartComponentId { get; set; }

        public Guid ParentId { get => AssembledComponentId; }
        public Guid AssociateId { get => PartComponentId; }

#nullable disable
        public ComponentPartAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentPartAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            AssembledComponentId = data.AssembledComponentId;
            PartComponentId = data.PartComponentId;
        }

        private void Apply(Marten.Events.Event<Events.ComponentPartRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(AssembledComponentId, nameof(AssembledComponentId)),
                    ValidateEmpty(PartComponentId, nameof(PartComponentId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(AssembledComponentId, nameof(AssembledComponentId)),
                  ValidateNonEmpty(PartComponentId, nameof(PartComponentId))
                  );
        }

        public Result<Models.ComponentPart, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentPart, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var assembledComponentIdResult = ValueObjects.Id.From(AssembledComponentId);
            var partComponentIdResult = ValueObjects.Id.From(PartComponentId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  assembledComponentIdResult,
                  partComponentIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentPart.From(
                    id: idResult.Value,
                    assembledComponentId: assembledComponentIdResult.Value,
                    partComponentId: partComponentIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}