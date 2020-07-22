using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;

namespace Infrastructure.Handlers
{
    public sealed class GetModelsAtTimestampsHandler<TModel, TAggregate, TCreatedEvent>
      : IQueryHandler<Queries.GetModelsAtTimestampsQuery<TModel>, IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
      where TModel : IModel
      where TAggregate : class, IAggregate, IConvertible<TModel>, new()
      where TCreatedEvent : ICreatedEvent
    {
        private readonly IModelRepository _repository;

        public GetModelsAtTimestampsHandler(IModelRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>> Handle(
            Queries.GetModelsAtTimestampsQuery<TModel> query,
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