using Infrastructure.Aggregates;
using Infrastructure.Models;

namespace Infrastructure.Handlers
{
    public abstract class GetManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      : GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TModel : IModel
      where TAssociationModel : IManyToManyAssociation
      where TAggregate : class, IAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    {
        protected GetManyToManyAssociationsOfModelsHandler(
            IModelRepository repository
            )
          : base(repository)
        {
        }
    }
}