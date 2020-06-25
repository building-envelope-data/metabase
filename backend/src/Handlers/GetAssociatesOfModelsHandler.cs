using Icon.Infrastructure.Aggregate;

namespace Icon.Handlers
{
    public abstract class GetAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      where TAssociationModel : Models.IAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, Aggregates.IAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    {
        protected readonly IAggregateRepository _repository;

        public GetAssociatesOfModelsHandler(
            IAggregateRepository repository
            )
        {
            _repository = repository;
        }
    }
}