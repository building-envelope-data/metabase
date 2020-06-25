using MediatR;
using Guid = System.Guid;

namespace Icon.Events
{
    public interface IEvent : INotification, IValidatable
    {
        public Guid CreatorId { get; }
    }
}