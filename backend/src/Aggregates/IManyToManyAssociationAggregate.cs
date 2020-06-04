using Guid = System.Guid;
using IEventSourcedAggregate = Icon.Infrastructure.Aggregate.IEventSourcedAggregate;

namespace Icon.Aggregates
{
    public interface IManyToManyAssociationAggregate
      : IEventSourcedAggregate
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}