using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Infrastructure.Events
{
    public abstract class DeletedEvent
      : Event, IDeletedEvent
    {
        public Guid AggregateId { get; set; }

#nullable disable
        protected DeletedEvent() { }
#nullable enable

        protected DeletedEvent(
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