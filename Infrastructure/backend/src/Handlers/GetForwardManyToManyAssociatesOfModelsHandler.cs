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
    public sealed class GetForwardManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>
      : GetManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>,
        IQueryHandler<Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
      where TModel : IModel
      where TAssociationModel : IManyToManyAssociation
      where TAssociateModel : IModel
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAddedEvent : IAssociationAddedEvent
    {
        public GetForwardManyToManyAssociatesOfModelsHandler(
            IModelRepository repository
            )
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}