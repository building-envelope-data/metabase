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
    public sealed class GetForwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>
      : GetManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
    {
        public static Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>> Do(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            CancellationToken cancellationToken
            )
        {
            return GetManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>.Do(
                session,
                timestampedModelIds,
                QueryAssociationIds,
                cancellationToken
                );
        }

        private static async Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associationId)>> QueryAssociationIds(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.Id> modelIds,
            CancellationToken cancellationToken
            )
        {
            var modelGuids = modelIds.Select(modelId => (Guid)modelId).ToArray();
            return (await session.QueryEvents<TAssociationAddedEvent>()
                .Where(e => e.ParentId.IsOneOf(modelGuids))
                .Select(e => new { ModelId = e.ParentId, AssociationId = e.AggregateId })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false))
              .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociationId));
        }

        public GetForwardManyToManyAssociationsOfModelsHandler(IAggregateRepository repository)
          : base(repository, QueryAssociationIds)
        {
        }
    }
}