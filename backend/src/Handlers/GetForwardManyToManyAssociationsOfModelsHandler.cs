using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Icon.Infrastructure.Events;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Handlers
{
    public sealed class GetForwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>
      : GetManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>,
        IQueryHandler<Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
    {
        public GetForwardManyToManyAssociationsOfModelsHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>> Handle(
            Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}