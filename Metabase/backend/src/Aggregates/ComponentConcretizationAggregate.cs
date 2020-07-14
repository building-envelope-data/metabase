using System;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Marten.Schema;
using Errors = Infrastructure.Errors;

namespace Metabase.Aggregates
{
    public sealed class ComponentConcretizationAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.ComponentConcretization>
    {
        // The instance variables `GeneralCompId` and `ConcreteCompId` are
        // abbreviated to make the corresponding database index have less than
        // 64 characters (which is Postgres maximum). I would prefer not
        // abbreviating the variable names but specifying custom database
        // index names instead.
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid GeneralCompId { get; set; }

        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ConcreteCompId { get; set; }

        public Guid ParentId { get => GeneralCompId; }
        public Guid AssociateId { get => ConcreteCompId; }

#nullable disable
        public ComponentConcretizationAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.ComponentConcretizationAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            GeneralCompId = data.GeneralComponentId;
            ConcreteCompId = data.ConcreteComponentId;
        }

        public void Apply(Marten.Events.Event<Events.ComponentConcretizationRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
            {
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(GeneralCompId, nameof(GeneralCompId)),
                    ValidateEmpty(ConcreteCompId, nameof(ConcreteCompId))
                    );
            }
            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(GeneralCompId, nameof(GeneralCompId)),
                  ValidateNonEmpty(ConcreteCompId, nameof(ConcreteCompId))
                  );
        }

        public Result<Models.ComponentConcretization, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentConcretization, Errors>(virginResult.Error);

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var generalComponentIdResult = Infrastructure.ValueObjects.Id.From(GeneralCompId);
            var concreteComponentIdResult = Infrastructure.ValueObjects.Id.From(ConcreteCompId);
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

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