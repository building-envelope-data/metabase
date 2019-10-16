using System;
using System.Collections.Generic;
using Icon.Infrastructure.Event;

namespace Icon.Infrastructure.Aggregate
{
    public interface IEventSourcedAggregate : IAggregate
    {
        // For protecting the state, i.e. conflict prevention
        public int Version { get; }

        public IEnumerable<IEvent> GetUncommittedEvents();

        public void ClearUncommittedEvents();
    }
}