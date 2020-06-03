using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using Marten;
using Marten.Linq.MatchesSql;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using Events = Icon.Events;
using Guid = System.Guid;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using Queries = Icon.Queries;

namespace Icon.Handlers
{
    public sealed class GetDatabasesOperatedByInstitutionsHandler
      : GetOneToManyAssociatesOfModelsHandler<Models.Institution, Models.Database, Aggregates.InstitutionAggregate, Aggregates.DatabaseAggregate, Events.DatabaseCreated>
    {
        public static Task<IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>> Do(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            CancellationToken cancellationToken
            )
        {
            return GetOneToManyAssociatesOfModelsHandler<Models.Institution, Models.Database, Aggregates.InstitutionAggregate, Aggregates.DatabaseAggregate, Events.DatabaseCreated>.Do(
                session,
                timestampedModelIds,
                QueryAssociateIds,
                cancellationToken
                );
        }

        private static async Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associateId)>> QueryAssociateIds(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.Id> modelIds,
            CancellationToken cancellationToken
            )
        {
            var modelGuids = modelIds.Select(modelId => (Guid)modelId).ToArray();
            return
              (await session.QueryEvents<Events.DatabaseCreated>()
                .Where(createdEvent => createdEvent.InstitutionId.IsOneOf(modelGuids))
                .Select(createdEvent => new { ModelId = createdEvent.InstitutionId, AssociateId = createdEvent.AggregateId })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false)
                )
              .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociateId));
        }

        public GetDatabasesOperatedByInstitutionsHandler(IAggregateRepository repository)
          : base(repository, QueryAssociateIds)
        {
        }
    }
}