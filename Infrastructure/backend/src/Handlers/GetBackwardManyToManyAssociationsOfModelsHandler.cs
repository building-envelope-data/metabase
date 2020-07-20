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
    public sealed class GetBackwardManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>
      : GetManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate>,
        IQueryHandler<Queries.GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>, IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
      where TAssociateModel : IModel
      where TAssociationModel : IManyToManyAssociation
      where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociationAddedEvent : IAssociationAddedEvent
    {
        public GetBackwardManyToManyAssociationsOfModelsHandler(IModelRepository repository)
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>> Handle(
            Queries.GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel> query,
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