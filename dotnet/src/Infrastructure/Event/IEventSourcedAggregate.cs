using System.Collections.Generic;
using Icon.Infrastructure.Event;

namespace Icon.Infrastructure.Aggregate
{
    public interface IEventSourcedAggregate : IAggregate
    {
        Queue<IEvent> PendingEvents { get; }
    }
}