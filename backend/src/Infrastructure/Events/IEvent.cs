using MediatR;
using Guid = System.Guid;

namespace Icon.Infrastructure.Events
{
    public interface IEvent : INotification, IValidatable
    {
        public Guid CreatorId { get; }
    }
}