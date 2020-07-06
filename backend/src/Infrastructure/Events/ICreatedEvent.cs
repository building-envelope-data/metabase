using Guid = System.Guid;

namespace Icon.Infrastructure.Events
{
    public interface ICreatedEvent
      : IEvent
    {
        public Guid AggregateId { get; }
    }
}