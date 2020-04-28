using Guid = System.Guid;
using Models = Icon.Models;
using Aggregates = Icon.Aggregates;
using System.Threading.Tasks;
using IAggregateRepository = Icon.Infrastructure.Aggregate.IAggregateRepository;
using IAggregateRepositoryReadOnlySession = Icon.Infrastructure.Aggregate.IAggregateRepositoryReadOnlySession;
using CancellationToken = System.Threading.CancellationToken;
using System.Collections.Generic;
using System.Linq; // Where, Select
using Marten; // IsOneOf

namespace Icon.Handlers
{
  public sealed class GetInstitutionsAffiliatedWithPersonsIdentifiedByTimestampedIdsHandler
    : GetAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Person, Models.Institution, Aggregates.InstitutionAggregate>
  {
    public GetInstitutionsAffiliatedWithPersonsIdentifiedByTimestampedIdsHandler(IAggregateRepository repository)
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
          .Where(e => e.PersonId.IsOneOf(modelGuids))
          .Select(e => new { ModelId = e.PersonId, AssociateId = e.InstitutionId })
          .ToListAsync(cancellationToken))
        .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociateId));
    }
  }
}
