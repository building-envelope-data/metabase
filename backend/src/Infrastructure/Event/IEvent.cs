using MediatR;
using Guid = System.Guid;

namespace Icon.Infrastructure.Event
{
    public interface IEvent : INotification
    {
        public Guid CreatorId { get; set; }
    }
}