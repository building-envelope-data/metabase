using MediatR;
using Guid = System.Guid;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Events
{
    public interface IEvent : INotification, IValidatable
    {
        public Guid CreatorId { get; }
    }
}