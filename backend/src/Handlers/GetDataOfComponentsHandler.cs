using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Icon.Infrastructure.Aggregates;
using Icon.Infrastructure.Queries;
using Marten;
using CancellationToken = System.Threading.CancellationToken;
using Guid = System.Guid;

namespace Icon.Handlers
{
    public sealed class GetDataOfComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>
      : IQueryHandler<Queries.GetDataOfComponents<TDataModel>, IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>>>
      where TDataAggregate : class, IEventSourcedAggregate, IConvertible<TDataModel>, new()
      where TDataCreatedEvent : DataCreatedEvent
    {
        private readonly IAggregateRepository _repository;

        public GetDataOfComponentsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>>> Handle(
            Queries.GetDataOfComponents<TDataModel> query,
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
                      Result.Ok<IEnumerable<Result<TDataModel, Errors>>, Errors>(
                        aggregateResults.Select(aggregateResult =>
                          aggregateResult.Bind(aggregate =>
                            aggregate.ToModel()
                            )
                          )
                        )
                      );
            }
        }
    }
}