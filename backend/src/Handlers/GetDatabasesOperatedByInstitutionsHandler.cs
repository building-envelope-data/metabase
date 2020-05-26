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
using System.Linq.Expressions;

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