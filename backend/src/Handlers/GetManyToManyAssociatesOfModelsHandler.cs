using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Models;

namespace Icon.Handlers
{
    public abstract class GetManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : GetAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      where TAssociationModel : IManyToManyAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    {
        public GetManyToManyAssociatesOfModelsHandler(
            IAggregateRepository repository
            )
          : base(repository)
        {
        }
    }
}