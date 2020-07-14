using Infrastructure.Aggregates;
using Infrastructure.Models;

namespace Infrastructure.Handlers
{
    public abstract class GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IAssociationAggregate, IConvertible<TAssociationModel>, new()
    {
        protected readonly IModelRepository _repository;

        protected GetAssociationsOfModelsHandler(
            IModelRepository repository
            )
        {
            _repository = repository;
        }
    }
}