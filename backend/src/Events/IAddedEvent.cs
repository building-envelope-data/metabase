using Guid = System.Guid;

namespace Icon.Events
{
    public interface IAddedEvent
      : ICreatedEvent
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}