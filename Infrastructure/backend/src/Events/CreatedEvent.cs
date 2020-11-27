using CSharpFunctionalExtensions;
using Guid = System.Guid;

namespace Infrastructure.Events
{
    public abstract class CreatedEvent
      : Event, ICreatedEvent
    {
        public Guid AggregateId { get; set; }

#nullable disable
        protected CreatedEvent() { }
#nullable enable

        protected CreatedEvent(
            Guid aggregateId,
            Guid creatorId
            )
          : base(creatorId)
        {
            AggregateId = aggregateId;
        }

        public override Result<bool, Errors> Validate()
        {
            return Result.Combine(
                base.Validate(),
                ValidateNonEmpty(AggregateId, nameof(AggregateId))
                );
        }
    }
}