using Guid = System.Guid;
using IEventSourcedAggregate = Icon.Infrastructure.Aggregates.IEventSourcedAggregate;

namespace Icon.Infrastructure.Aggregates
{
    public interface IAssociationAggregate
      : IEventSourcedAggregate
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}