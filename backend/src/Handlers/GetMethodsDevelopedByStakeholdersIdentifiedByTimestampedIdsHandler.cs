using Models = Icon.Models;
using Events = Icon.Events;
using Aggregates = Icon.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;
using Guid = System.Guid;
using IAggregateRepository = Icon.Infrastructure.Aggregate.IAggregateRepository;
using IAggregateRepositoryReadOnlySession = Icon.Infrastructure.Aggregate.IAggregateRepositoryReadOnlySession;
using CancellationToken = System.Threading.CancellationToken;
using System.Linq; // Where, Select
using Marten; // IsOneOf

namespace Icon.Handlers
{
    public sealed class GetMethodsDevelopedByStakeholdersIdentifiedByTimestampedIdsHandler<TStakeholder, TAddedEvent>
      : GetAssociatesOfModelsIdentifiedByTimestampedIdsHandler<TStakeholder, Models.Method, Aggregates.MethodAggregate>
      where TAddedEvent : Events.MethodDeveloperAdded
    {
        public GetMethodsDevelopedByStakeholdersIdentifiedByTimestampedIdsHandler(IAggregateRepository repository)
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
                .Where(e => e.StakeholderId.IsOneOf(modelGuids))
                .Select(e => new { ModelId = e.StakeholderId, AssociateId = e.MethodId })
                .ToListAsync(cancellationToken))
              .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociateId));
        }
    }
}