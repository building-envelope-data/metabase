using System;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregates;

namespace Icon.Aggregates
{
    public abstract class DataAggregate
      : EventSourcedAggregate
    {
        public Guid ComponentId { get; set; }
        public object? Data { get; set; }

#nullable disable
        public DataAggregate() { }
#nullable enable

        protected void ApplyCreated<E>(Marten.Events.Event<E> @event)
          where E : Events.DataCreatedEvent
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            ComponentId = data.ComponentId;
            Data = data.Data;
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(ComponentId, nameof(ComponentId)),
                    ValidateNull(Data, nameof(Data))
                    );

            return Result.Combine(
                base.Validate(),
                ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                ValidateNonNull(Data, nameof(Data))
                );
        }
    }
}