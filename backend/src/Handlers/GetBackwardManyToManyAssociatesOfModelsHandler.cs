using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Icon.Infrastructure.Events;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Handlers
{
    public sealed class GetBackwardManyToManyAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>
      : GetManyToManyAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate>,
        IQueryHandler<Queries.GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>, IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
      where TAssociationModel : Models.IManyToManyAssociation
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAddedEvent : IAssociationAddedEvent
    {
        public GetBackwardManyToManyAssociatesOfModelsHandler(
            IAggregateRepository repository
            )
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>> Handle(
            Queries.GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}