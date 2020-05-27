using Guid = System.Guid;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.Events
{
    public abstract class RemovedEvent
      : DeletedEvent, IRemovedEvent
    {
#nullable disable
        public RemovedEvent() { }
#nullable enable

        public RemovedEvent(
            Guid aggregateId,
            Guid creatorId
            )
          : base(
              aggregateId: aggregateId,
              creatorId: creatorId
              )
        {
        }
    }
}