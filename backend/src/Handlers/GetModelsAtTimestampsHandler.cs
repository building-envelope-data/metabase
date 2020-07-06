using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregates;
using Icon.Infrastructure.Queries;
using Icon.Infrastructure.Events;
using CancellationToken = System.Threading.CancellationToken;

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
                return await session.GetModelsAtTimestamps<TModel, TAggregate, TCreatedEvent>(
                    query.Timestamps,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}