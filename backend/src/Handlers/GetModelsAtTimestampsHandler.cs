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