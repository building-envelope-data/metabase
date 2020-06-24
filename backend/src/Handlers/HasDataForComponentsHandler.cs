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
                   .Select(e => new { ComponentId = e.ComponentId, DataModelId = e.AggregateId })
                   .ToListAsync(cancellationToken)
                   .ConfigureAwait(false)
                  )
                  .ToLookup(
                      x => (ValueObjects.Id)x.ComponentId,
                      x => (ValueObjects.Id)x.DataModelId
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