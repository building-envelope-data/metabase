using MediatR;

namespace Icon.Infrastructure.Event
{
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
           where TEvent : IEvent
    {
    }
}