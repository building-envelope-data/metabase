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
    public sealed class GetForwardOneToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>
      : GetOneToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>,
        IQueryHandler<Queries.GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>, IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
          where TModel : IModel
          where TAssociationModel : IOneToManyAssociation
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : IAssociationAddedEvent
    {
        public GetForwardOneToManyAssociationsOfModelsHandler(IModelRepository repository)
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>> Handle(
            Queries.GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}