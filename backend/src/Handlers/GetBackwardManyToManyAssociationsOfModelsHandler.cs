using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Icon.Infrastructure.Events;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Handlers
{
    public sealed class GetBackwardManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>
      : GetManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate>,
        IQueryHandler<Queries.GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>, IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociationAddedEvent : IAssociationAddedEvent
    {
        public GetBackwardManyToManyAssociationsOfModelsHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>> Handle(
            Queries.GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}