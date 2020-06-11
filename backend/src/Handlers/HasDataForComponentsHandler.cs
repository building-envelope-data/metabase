using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using Marten;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using Guid = System.Guid;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using Queries = Icon.Queries;

namespace Icon.Handlers
{
    public sealed class HasDataForComponentsHandler<TComponentDataAssociationModel, TDataModel, TComponentDataAssociationAggregate, TDataAggregate, TAssociationAddedEvent>
      : IQueryHandler<Queries.HasDataForComponents<TDataModel>, IEnumerable<Result<bool, Errors>>>
      where TComponentDataAssociationModel : Models.IOneToManyAssociation
      where TComponentDataAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TComponentDataAssociationModel>, new()
      where TDataAggregate : class, IEventSourcedAggregate, IConvertible<TDataModel>, new()
      where TAssociationAddedEvent : Events.IAssociationAddedEvent
    {
        private readonly IAggregateRepository _repository;

        public HasDataForComponentsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<bool, Errors>>> Handle(
            Queries.HasDataForComponents<TDataModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Handle(session, query.TimestampedIds, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Result<bool, Errors>>> Handle(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            return
              (await session
               .GetForwardOneToManyAssociatesOfModels<Models.Component, TComponentDataAssociationModel, TDataModel, Aggregates.ComponentAggregate, TComponentDataAssociationAggregate, TDataAggregate, TAssociationAddedEvent>(
                 timestampedIds,
                 cancellationToken: cancellationToken
                 )
               .ConfigureAwait(false)
              )
              .Select(associateResultsResult =>
                  associateResultsResult.Map(associateResults =>
                    associateResults.Any()
                    )
                );
        }
    }
}