using Infrastructure.Aggregates;

namespace Infrastructure.Handlers
{
    public abstract class GetOneToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      : GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    {
        protected GetOneToManyAssociationsOfModelsHandler(
            IAggregateRepository repository
            )
          : base(repository)
        {
        }
    }
}