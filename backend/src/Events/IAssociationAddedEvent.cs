using Guid = System.Guid;

namespace Icon.Events
{
    public interface IAssociationAddedEvent
      : ICreatedEvent
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}