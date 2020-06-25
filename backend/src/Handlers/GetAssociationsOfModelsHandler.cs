using Icon.Infrastructure.Aggregate;

namespace Icon.Handlers
{
    public abstract class GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, Aggregates.IAssociationAggregate, IConvertible<TAssociationModel>, new()
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