using CSharpFunctionalExtensions;
using Marten.Schema;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentConcretizationAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.ComponentConcretization>
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid GeneralComponentId { get; set; }

        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ConcreteComponentId { get; set; }

        public Guid ParentId { get => GeneralComponentId; }
        public Guid AssociateId { get => ConcreteComponentId; }

#nullable disable
        public ComponentConcretizationAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentConcretizationAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            GeneralComponentId = data.GeneralComponentId;
            ConcreteComponentId = data.ConcreteComponentId;
        }

        private void Apply(Marten.Events.Event<Events.ComponentConcretizationRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(GeneralComponentId, nameof(GeneralComponentId)),
                    ValidateEmpty(ConcreteComponentId, nameof(ConcreteComponentId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(GeneralComponentId, nameof(GeneralComponentId)),
                  ValidateNonEmpty(ConcreteComponentId, nameof(ConcreteComponentId))
                  );
        }

        public Result<Models.ComponentConcretization, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentConcretization, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var generalComponentIdResult = ValueObjects.Id.From(GeneralComponentId);
            var concreteComponentIdResult = ValueObjects.Id.From(ConcreteComponentId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  generalComponentIdResult,
                  concreteComponentIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentConcretization.From(
                    id: idResult.Value,
                    generalComponentId: generalComponentIdResult.Value,
                    concreteComponentId: concreteComponentIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}