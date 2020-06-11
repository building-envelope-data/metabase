using Guid = System.Guid;
using IEventSourcedAggregate = Icon.Infrastructure.Aggregate.IEventSourcedAggregate;

namespace Icon.Aggregates
{
    public interface IAssociationAggregate
      : IEventSourcedAggregate
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}