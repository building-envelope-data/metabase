namespace Icon.Models
{
    public interface IAssociation
      : IModel
    {
        public ValueObjects.Id ParentId { get; }
        public ValueObjects.Id AssociateId { get; }
    }
}