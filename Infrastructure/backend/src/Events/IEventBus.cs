using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public interface IEventBus
    {
        public Task Publish<TEvent>(IEnumerable<TEvent> events)
          where TEvent : IEvent;

        public Task Publish<TEvent>(params TEvent[] events)
          where TEvent : IEvent;
    }
}