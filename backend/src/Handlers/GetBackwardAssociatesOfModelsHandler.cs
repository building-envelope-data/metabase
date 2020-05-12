using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Events = Icon.Events;
using Aggregates = Icon.Aggregates;
using System.Linq;
using Marten;
using Marten.Linq.MatchesSql;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System;

namespace Icon.Handlers
{
    public sealed class GetBackwardAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TModelAggregate, TAddedEvent>
      : GetAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TModelAggregate>,
        IQueryHandler<Queries.GetBackwardAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>, IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
      where TModelAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAddedEvent : Events.IAddedEvent
    {
        public GetBackwardAssociatesOfModelsHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        public Task<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>> Handle(
            Queries.GetBackwardAssociatesOfModels<TAssociateModel, TAssociationModel, TModel> query,
            CancellationToken cancellationToken
            )
        {
            return base.Handle(query, cancellationToken);
        }

        protected override async Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associateId)>> QueryAssociateIds(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.Id> modelIds,
            CancellationToken cancellationToken
            )
        {
            var modelGuids = modelIds.Select(modelId => (Guid)modelId).ToArray();
            return (await session.Query<TAddedEvent>()
                .Where(e => e.AssociateId.IsOneOf(modelGuids))
                .Select(e => new { ModelId = e.AssociateId, AssociateId = e.ParentId })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false))
              .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociateId));
        }
    }
}