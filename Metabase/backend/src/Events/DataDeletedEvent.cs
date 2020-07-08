using Infrastructure.Events;
using Guid = System.Guid;

namespace Metabase.Events
{
    public abstract class DataDeletedEvent
      : DeletedEvent, IDeletedEvent
    {
#nullable disable
        protected DataDeletedEvent() { }
#nullable enable

        protected DataDeletedEvent(
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