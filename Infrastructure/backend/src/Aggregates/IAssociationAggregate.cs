using Guid = System.Guid;
using IEventSourcedAggregate = Infrastructure.Aggregates.IEventSourcedAggregate;

namespace Infrastructure.Aggregates
{
    public interface IAssociationAggregate
      : IEventSourcedAggregate
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}