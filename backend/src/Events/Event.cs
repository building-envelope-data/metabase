using System.Collections.Generic;
using Guid = System.Guid;

namespace Icon.Events
{
    public abstract class Event : IEvent
    {
        public static void EnsureValid(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                @event.EnsureValid();
            }
        }

        public Guid CreatorId { get; set; }

        public Event() { }

        public Event(Guid creatorId)
        {
            CreatorId = creatorId;
        }

        public virtual bool IsValid()
        {
            return CreatorId != Guid.Empty;
        }
    }
}