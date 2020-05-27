using Guid = System.Guid;

namespace Icon.Aggregates
{
    public interface IManyToManyAssociationAggregate
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}