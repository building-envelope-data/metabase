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
    public sealed class GetModelsForTimestampedIdsHandler<M, A>
      : IQueryHandler<Queries.GetModelsForTimestampedIds<M>, IEnumerable<Result<M, Errors>>>,
        IGetModelsForTimestampedIdsHandler
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
                return await Handle(query.TimestampedIds, session, cancellationToken).ConfigureAwait(false);
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
               .ConfigureAwait(false)
              )
              .Select(result =>
                result.Bind(a => a.ToModel())
                );
        }

        public async Task<IEnumerable<Result<Models.IModel, Errors>>> HandleX(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            )
        {
            return
              (await Handle(timestampedIds, session, cancellationToken)
               .ConfigureAwait(false)
               )
              .Select(r => r.Map(m => (Models.IModel)m));
        }
    }
}