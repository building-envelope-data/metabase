using Infrastructure.Aggregates;
using Infrastructure.Models;

namespace Infrastructure.Handlers
{
    public abstract class GetAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      where TAssociationModel : IAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    {
        protected readonly IAggregateRepository _repository;

        protected GetAssociatesOfModelsHandler(
            IAggregateRepository repository
            )
        {
            _repository = repository;
        }
    }
}