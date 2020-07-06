using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Marten;
using Metabase.Events;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Metabase.Handlers
{
    public sealed class HasDataForComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>
      : IQueryHandler<Queries.HasDataForComponents<TDataModel>, IEnumerable<Result<bool, Errors>>>
      where TDataAggregate : class, IEventSourcedAggregate, IConvertible<TDataModel>, new()
      where TDataCreatedEvent : DataCreatedEvent
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
                var componentIds =
                  query.TimestampedIds
                  .Select(timestampedId =>
                      (Guid)timestampedId.Id
                      )
                  .ToArray();
                // TODO This is only correct when data aggregates are immutable!
                var componentIdToDataModelIds =
                  (await session.QueryEvents<TDataCreatedEvent>()
                   .Where(e => e.ComponentId.IsOneOf(componentIds))
                   .Select(e => new { e.ComponentId, DataModelId = e.AggregateId })
                   .ToListAsync(cancellationToken)
                   .ConfigureAwait(false)
                  )
                  .ToLookup(
                      x => (Id)x.ComponentId,
                      x => (Id)x.DataModelId
                      );
                return
                  (
                   // TODO Avoid loading and discarding deleted data aggregates.
                   await session.LoadAllThatExistedBatched<TDataAggregate>(
                     query.TimestampedIds.Select(timestampedId =>
                       (
                        timestampedId.Timestamp,
                        componentIdToDataModelIds[timestampedId.Id]
                       )
                       ),
                     cancellationToken
                     )
                   .ConfigureAwait(false)
                  )
                  .Select(aggregateResults =>
                      // TODO Determine the number of existing data aggregates without loading them!
                      Result.Ok<bool, Errors>(
                        aggregateResults.Count() >= 1
                        )
                      );
            }
        }
    }
}