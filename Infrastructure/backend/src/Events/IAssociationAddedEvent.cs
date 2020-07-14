using Guid = System.Guid;

namespace Infrastructure.Events
{
    public interface IAssociationAddedEvent
      : ICreatedEvent
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}