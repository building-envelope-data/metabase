using Guid = System.Guid;
using IEventSourcedAggregate = Infrastructure.Aggregates.IAggregate;

namespace Infrastructure.Aggregates
{
    public interface IAssociationAggregate
      : IAggregate
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}