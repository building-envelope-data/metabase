using Guid = System.Guid;
using Models = Icon.Models;
using System.Linq; // Where, Select
using Marten; // IsOneOf
using Aggregates = Icon.Aggregates;
using System.Threading.Tasks;
using IAggregateRepository = Icon.Infrastructure.Aggregate.IAggregateRepository;
using IAggregateRepositoryReadOnlySession = Icon.Infrastructure.Aggregate.IAggregateRepositoryReadOnlySession;
using CancellationToken = System.Threading.CancellationToken;
using System.Collections.Generic;

namespace Icon.Handlers
{
  public sealed class GetPersonsAffiliatedWithInstitutionsIdentifiedByTimestampedIdsHandler
    : GetAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.Person, Aggregates.PersonAggregate>
  {
    public GetPersonsAffiliatedWithInstitutionsIdentifiedByTimestampedIdsHandler(IAggregateRepository repository)
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
      return (await session.Query<Events.PersonAffiliationAdded>()
          .Where(e => e.InstitutionId.IsOneOf(modelGuids))
          .Select(e => new { ModelId = e.InstitutionId, AssociateId = e.PersonId })
          .ToListAsync(cancellationToken))
        .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociateId));
    }
  }
}
