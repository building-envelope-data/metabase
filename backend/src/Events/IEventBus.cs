using System.Threading.Tasks;
using System.Collections.Generic;

namespace Icon.Events
{
    public interface IEventBus
    {
        public Task Publish<TEvent>(IEnumerable<TEvent> events)
          where TEvent : IEvent;

        public Task Publish<TEvent>(params TEvent[] events)
          where TEvent : IEvent;
    }
}