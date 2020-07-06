using CSharpFunctionalExtensions;
using Guid = System.Guid;

namespace Icon.Infrastructure.Events
{
    public abstract class DeletedEvent
      : Event, IDeletedEvent
    {
        public Guid AggregateId { get; set; }

#nullable disable
        public DeletedEvent() { }
#nullable enable

        public DeletedEvent(
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