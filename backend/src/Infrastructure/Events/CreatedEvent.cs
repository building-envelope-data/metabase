using CSharpFunctionalExtensions;
using Guid = System.Guid;

namespace Icon.Infrastructure.Events
{
    public abstract class CreatedEvent
      : Event, ICreatedEvent
    {
        public Guid AggregateId { get; set; }

#nullable disable
        public CreatedEvent() { }
#nullable enable

        public CreatedEvent(
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