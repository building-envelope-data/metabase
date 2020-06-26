using System;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;

namespace Icon.Aggregates
{
    public sealed class ComponentVariantAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.ComponentVariant>
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid BaseComponentId { get; set; }

        [ForeignKey(typeof(ComponentAggregate))]
        public Guid VariantComponentId { get; set; }

        public Guid ParentId { get => BaseComponentId; }
        public Guid AssociateId { get => VariantComponentId; }

#nullable disable
        public ComponentVariantAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.ComponentVariantAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            BaseComponentId = data.BaseComponentId;
            VariantComponentId = data.VariantComponentId;
        }

        public void Apply(Marten.Events.Event<Events.ComponentVariantRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(BaseComponentId, nameof(BaseComponentId)),
                    ValidateEmpty(VariantComponentId, nameof(VariantComponentId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(BaseComponentId, nameof(BaseComponentId)),
                  ValidateNonEmpty(VariantComponentId, nameof(VariantComponentId))
                  );
        }

        public Result<Models.ComponentVariant, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentVariant, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var baseComponentIdResult = ValueObjects.Id.From(BaseComponentId);
            var variantComponentIdResult = ValueObjects.Id.From(VariantComponentId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  baseComponentIdResult,
                  variantComponentIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentVariant.From(
                    id: idResult.Value,
                    baseComponentId: baseComponentIdResult.Value,
                    variantComponentId: variantComponentIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}