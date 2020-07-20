using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Handlers
{
    public sealed class GetBackwardOneToManyAssociationOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>
      : GetOneToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate>,
        IQueryHandler<Queries.GetBackwardOneToManyAssociationOfModelsQuery<TAssociateModel, TAssociationModel>, IEnumerable<Result<TAssociationModel, Errors>>>
      where TAssociateModel : IModel
      where TAssociationModel : IOneToManyAssociation
      where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociationAddedEvent : IAssociationAddedEvent
    {
        public GetBackwardOneToManyAssociationOfModelsHandler(IModelRepository repository)
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<TAssociationModel, Errors>>> Handle(
            Queries.GetBackwardOneToManyAssociationOfModelsQuery<TAssociateModel, TAssociationModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}