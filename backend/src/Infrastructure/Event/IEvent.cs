using MediatR;
using Guid = System.Guid;

namespace Icon.Infrastructure.Event
{
    public interface IEvent : INotification, IValidatable
    {
        public Guid CreatorId { get; }
    }
}