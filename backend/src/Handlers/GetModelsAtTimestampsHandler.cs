using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Marten;
using DateTime = System.DateTime;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Aggregates = Icon.Aggregates;
using System.Linq;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public sealed class GetModelsAtTimestampsHandler<TModel, TAggregate, TCreatedEvent>
      : IQueryHandler<Queries.GetModelsAtTimestamps<TModel>, IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TCreatedEvent : ICreatedEvent
    {
        private readonly IAggregateRepository _repository;

        public GetModelsAtTimestampsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>> Handle(
            Queries.GetModelsAtTimestamps<TModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                var possibleIds = await QueryModelIds(session, cancellationToken);
                return
                  (await session
                   .LoadAllThatExistedBatched<TAggregate>(
                     query.Timestamps.Select(timestamp => (timestamp, possibleIds)),
                     cancellationToken
                     )
                    ).Select(results =>
                      Result.Ok<IEnumerable<Result<TModel, Errors>>, Errors>(
                        results.Select(result =>
                          result.Bind(a => a.ToModel())
                          )
                        )
                      );
            }
        }

        private async Task<IEnumerable<ValueObjects.Id>> QueryModelIds(
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            )
        {
            return (await session.Query<TCreatedEvent>()
              .Select(e => e.AggregateId)
              .ToListAsync(cancellationToken))
              .Select(id => (ValueObjects.Id)id);
        }
    }
}