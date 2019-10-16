using Guid = System.Guid;

namespace Icon.Infrastructure.Event
{
    public abstract class EventBase : IEvent
    {
        public Guid CreatorId { get; set; }
    }
}