using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Icon.Infrastructure.Events;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Handlers
{
    public sealed class GetBackwardOneToManyAssociateOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>
      : GetOneToManyAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate>,
        IQueryHandler<Queries.GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>, IEnumerable<Result<TModel, Errors>>>
      where TAssociationModel : Models.IOneToManyAssociation
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAddedEvent : IAssociationAddedEvent
    {
        public GetBackwardOneToManyAssociateOfModelsHandler(
            IAggregateRepository repository
            )
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<TModel, Errors>>> Handle(
            Queries.GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}