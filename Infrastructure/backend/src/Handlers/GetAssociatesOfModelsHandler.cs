using Infrastructure.Aggregates;
using Infrastructure.Models;

namespace Infrastructure.Handlers
{
    public abstract class GetAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      where TModel : IModel
      where TAssociationModel : IAssociation
      where TAssociateModel : IModel
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    {
        protected readonly IModelRepository _repository;

        protected GetAssociatesOfModelsHandler(
            IModelRepository repository
            )
        {
            _repository = repository;
        }
    }
}