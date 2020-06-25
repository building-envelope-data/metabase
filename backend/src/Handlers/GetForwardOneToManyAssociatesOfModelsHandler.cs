using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Handlers
{
    public sealed class GetForwardOneToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>
      : GetOneToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>,
        IQueryHandler<Queries.GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
      where TAssociationModel : Models.IOneToManyAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAddedEvent : Events.IAssociationAddedEvent
    {
        public GetForwardOneToManyAssociatesOfModelsHandler(
            IAggregateRepository repository
            )
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            Queries.GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}