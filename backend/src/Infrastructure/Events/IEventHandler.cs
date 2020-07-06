using MediatR;

namespace Icon.Infrastructure.Events
{
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
           where TEvent : IEvent
    {
    }
}