using Icon.Infrastructure.Aggregate;

namespace Icon.Handlers
{
    public abstract class GetOneToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      : GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    {
        public GetOneToManyAssociationsOfModelsHandler(
            IAggregateRepository repository
            )
          : base(repository)
        {
        }
    }
}