using Guid = System.Guid;

namespace Icon.Events
{
    public interface ICreatedEvent
      : IEvent
    {
        public Guid AggregateId { get; }
    }
}