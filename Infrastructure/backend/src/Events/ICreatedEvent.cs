using Guid = System.Guid;

namespace Infrastructure.Events
{
    public interface ICreatedEvent
      : IEvent
    {
        public Guid AggregateId { get; }
    }
}