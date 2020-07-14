using Infrastructure.ValueObjects;
namespace Infrastructure.Models
{
    public interface IAssociation
      : IModel
    {
        public Id ParentId { get; }
        public Id AssociateId { get; }
    }
}