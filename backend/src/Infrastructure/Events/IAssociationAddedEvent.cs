using Guid = System.Guid;

namespace Icon.Infrastructure.Events
{
    public interface IAssociationAddedEvent
      : ICreatedEvent
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}