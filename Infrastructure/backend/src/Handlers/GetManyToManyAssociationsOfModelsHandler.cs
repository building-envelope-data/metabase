using Infrastructure.Aggregates;

namespace Infrastructure.Handlers
{
    public abstract class GetManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      : GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    {
        protected GetManyToManyAssociationsOfModelsHandler(
            IAggregateRepository repository
            )
          : base(repository)
        {
        }
    }
}