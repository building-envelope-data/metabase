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
    public sealed class GetForwardAssociationsOfModelsHandler<TModel, TAssociationModel, TAssociationAggregate, TAddedEvent>
      : GetAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociationModel, TAssociationAggregate>
            where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
            where TAddedEvent : Events.IAddedEvent
    {
        public GetForwardAssociationsOfModelsHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override async Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associateId)>> QueryAssociateIds(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.Id> modelIds,
            CancellationToken cancellationToken
            )
        {
            var modelGuids = modelIds.Select(modelId => (Guid)modelId).ToArray();
            return (await session.Query<TAddedEvent>()
                .Where(e => e.ParentId.IsOneOf(modelGuids))
                .Select(e => new { ModelId = e.ParentId, AssociationId = e.AggregateId })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false))
              .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociationId));
        }
    }
}