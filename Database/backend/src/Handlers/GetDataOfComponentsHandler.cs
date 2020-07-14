using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Models;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Marten;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Database.Handlers
{
    public sealed class GetDataOfComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>
      : IQueryHandler<Queries.GetDataOfComponents<TDataModel>, IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>>>
      where TDataModel : IModel
      where TDataAggregate : class, IEventSourcedAggregate, IConvertible<TDataModel>, new()
      where TDataCreatedEvent : Events.DataCreatedEvent
    {
        private readonly IModelRepository _repository;

        public GetDataOfComponentsHandler(IModelRepository repository)
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
                      x => (Id)x.ComponentId,
                      x => (Id)x.DataModelId
                      );
                return
                  (
                   // TODO Avoid loading and discarding deleted data models.
                   await session.LoadAllThatExistedBatched<TDataModel, TDataAggregate>(
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
                  .Select(modelResults =>
                      Result.Ok<IEnumerable<Result<TDataModel, Errors>>, Errors>(
                        modelResults
                        )
                      );
            }
        }
    }
}