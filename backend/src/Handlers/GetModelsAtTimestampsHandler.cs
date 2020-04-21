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
    public abstract class GetModelsAtTimestampsHandler<M, A>
      : IQueryHandler<Queries.GetModelsAtTimestamps<M>, IEnumerable<Result<IEnumerable<Result<M, Errors>>, Errors>>>
      where A : class, IEventSourcedAggregate, IConvertible<M>, new()
    {
        private readonly IAggregateRepository _repository;

        public GetModelsAtTimestampsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<M, Errors>>, Errors>>> Handle(
            Queries.GetModelsAtTimestamps<M> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                var possibleIds = await QueryModelIds(session, cancellationToken);
                return
                  (await session
                   .LoadAllThatExistedBatched<A>(
                     query.Timestamps.Select(timestamp => (timestamp, possibleIds)),
                     cancellationToken
                     )
                    ).Select(results =>
                      Result.Ok<IEnumerable<Result<M, Errors>>, Errors>(
                        results.Select(result =>
                          result.Bind(a => a.ToModel())
                          )
                        )
                      );
            }
        }

        protected abstract Task<IEnumerable<ValueObjects.Id>> QueryModelIds(
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            );
    }
}