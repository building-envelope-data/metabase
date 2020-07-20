using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Models;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;

namespace Infrastructure.Handlers
{
    public sealed class GetModelsForTimestampedIdsHandler<TModel, TAggregate>
      : IQueryHandler<Queries.GetModelsForTimestampedIdsQuery<TModel>, IEnumerable<Result<TModel, Errors>>>,
        IGetModelsForTimestampedIdsHandler
      where TModel : IModel
      where TAggregate : class, IAggregate, IConvertible<TModel>, new()
    {
        private readonly IModelRepository _repository;

        public GetModelsForTimestampedIdsHandler(IModelRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<TModel, Errors>>> Handle(
            Queries.GetModelsForTimestampedIdsQuery<TModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Handle(session, query.TimestampedIds, cancellationToken).ConfigureAwait(false);
            }
        }

        public Task<IEnumerable<Result<TModel, Errors>>> Handle(
            ModelRepositoryReadOnlySession session,
            IEnumerable<TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            return session.LoadAll<TModel, TAggregate>(
                timestampedIds,
                cancellationToken: cancellationToken
                );
        }

        public async Task<IEnumerable<Result<IModel, Errors>>> HandleX(
            ModelRepositoryReadOnlySession session,
            IEnumerable<TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            return
              (await Handle(session, timestampedIds, cancellationToken)
               .ConfigureAwait(false)
               )
              .Select(r => r.Map(m => (IModel)m));
        }
    }
}