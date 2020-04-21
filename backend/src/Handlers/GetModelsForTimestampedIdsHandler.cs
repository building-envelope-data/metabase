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
    public sealed class GetModelsForTimestampedIdsHandler<M, A>
      : IQueryHandler<Queries.GetModelsForTimestampedIds<M>, IEnumerable<Result<M, Errors>>>
      , IGetModelsForTimestampedIdsHandler
      where M : Models.IModel
      where A : class, IEventSourcedAggregate, IConvertible<M>, new()
    {
        private readonly IAggregateRepository _repository;

        public GetModelsForTimestampedIdsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<M, Errors>>> Handle(
            Queries.GetModelsForTimestampedIds<M> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Handle(query.TimestampedIds, session, cancellationToken);
            }
        }

        public async Task<IEnumerable<Result<M, Errors>>> Handle(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            )
        {
            return
              (await session
               .LoadAll<A>(
                 timestampedIds,
                 cancellationToken: cancellationToken
                 )
              ).Select(result =>
                result.Bind(a => a.ToModel())
                );
        }

        public async Task<IEnumerable<Result<Models.IModel, Errors>>> HandleX(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            )
        {
            return (await Handle(timestampedIds, session, cancellationToken))
              .Select(r => r.Map(m => (Models.IModel)m));
        }
    }
}