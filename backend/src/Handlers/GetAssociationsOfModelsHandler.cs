using Icon.Infrastructure.Aggregates;

namespace Icon.Handlers
{
    public abstract class GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IAssociationAggregate, IConvertible<TAssociationModel>, new()
    {
        protected readonly IAggregateRepository _repository;

        public GetAssociationsOfModelsHandler(
            IAggregateRepository repository
            )
        {
            _repository = repository;
        }
    }
}