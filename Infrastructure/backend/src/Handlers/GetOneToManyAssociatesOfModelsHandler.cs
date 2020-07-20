using Infrastructure.Aggregates;
using Infrastructure.Models;

namespace Infrastructure.Handlers
{
    public abstract class GetOneToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : GetAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      where TModel : IModel
      where TAssociationModel : IOneToManyAssociation
      where TAssociateModel : IModel
      where TAggregate : class, IAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
    {
        protected GetOneToManyAssociatesOfModelsHandler(
            IModelRepository repository
            )
          : base(repository)
        {
        }
    }
}