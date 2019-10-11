using System.Threading.Tasks;

namespace Icon.Infrastructure.Event
{
    public interface IEventBus
    {
        Task Publish<TEvent>(params TEvent[] events) where TEvent : IEvent;
    }
}