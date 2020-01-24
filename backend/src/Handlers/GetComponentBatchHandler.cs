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
    public class GetComponentBatchHandler
      : IQueryHandler<Queries.GetComponentBatch, IEnumerable<Result<Models.Component, Errors>>>
    {
        private readonly IAggregateRepository _repository;

        public GetComponentBatchHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<Models.Component, Errors>>> Handle(
            Queries.GetComponentBatch query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return
                  (await session
                   .LoadAll<Aggregates.ComponentAggregate>(
                     query.TimestampedComponentIds,
                     cancellationToken: cancellationToken
                     )
                  ).Select(result =>
                    result.Bind(a => a.ToModel())
                    );
            }
        }
    }
}