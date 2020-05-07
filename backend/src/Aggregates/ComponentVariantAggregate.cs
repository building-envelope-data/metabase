using CSharpFunctionalExtensions;
using Marten.Schema;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentVariantAggregate
      : EventSourcedAggregate, IConvertible<Models.ComponentVariant>
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid BaseComponentId { get; set; }

        [ForeignKey(typeof(ComponentAggregate))]
        public Guid VariantComponentId { get; set; }

#nullable disable
        public ComponentVariantAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentVariantAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            BaseComponentId = data.BaseComponentId;
            VariantComponentId = data.VariantComponentId;
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