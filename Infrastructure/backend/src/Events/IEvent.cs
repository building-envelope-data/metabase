using MediatR;
using Guid = System.Guid;
using IValidatable = Infrastructure.IValidatable;

namespace Infrastructure.Events
{
    public interface IEvent : INotification, IValidatable
    {
        public Guid CreatorId { get; }
    }
}