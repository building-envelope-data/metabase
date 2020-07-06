using Infrastructure.Events;
using Guid = System.Guid;

namespace Metabase.Events
{
    public abstract class DataDeletedEvent
      : DeletedEvent, IDeletedEvent
    {
#nullable disable
        public DataDeletedEvent() { }
#nullable enable

        public DataDeletedEvent(
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