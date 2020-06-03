using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace Icon.Events
{
    public class EventBus : IEventBus
    {
        private readonly IMediator _mediator;

        public EventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish<TEvent>(IEnumerable<TEvent> events)
          where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                await _mediator.Publish(@event).ConfigureAwait(false);
            }
        }

        public Task Publish<TEvent>(params TEvent[] events)
          where TEvent : IEvent
        {
            return Publish<TEvent>(events);
        }
    }
}