using Guid = System.Guid;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.Events
{
    public abstract class CreatedEvent
      : Event
    {
      public Guid AggregateId { get; }

#nullable disable
        public CreatedEvent() { }
#nullable enable

        public CreatedEvent(Guid aggregateId, Guid creatorId)
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
