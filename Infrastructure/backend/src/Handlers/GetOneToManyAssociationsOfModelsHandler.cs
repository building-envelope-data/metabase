using Infrastructure.Aggregates;
using Infrastructure.Models;

namespace Infrastructure.Handlers
{
    public abstract class GetOneToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      : GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TModel : IModel
      where TAssociationModel : IOneToManyAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    {
        protected GetOneToManyAssociationsOfModelsHandler(
            IModelRepository repository
            )
          : base(repository)
        {
        }
    }
}