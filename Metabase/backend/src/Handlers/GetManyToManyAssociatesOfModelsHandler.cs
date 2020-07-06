using Infrastructure.Aggregates;
using Infrastructure.Models;

namespace Metabase.Handlers
{
    public abstract class GetManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : GetAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      where TAssociationModel : IManyToManyAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
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