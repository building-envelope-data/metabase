using Guid = System.Guid;

namespace Icon.Events
{
    public interface IDeletedEvent
      : IEvent
    {
        public Guid AggregateId { get; }
    }
}