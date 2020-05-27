namespace Icon.Models
{
    public interface IManyToManyAssociation
      : IModel
    {
        public ValueObjects.Id ParentId { get; }
        public ValueObjects.Id AssociateId { get; }
    }
}