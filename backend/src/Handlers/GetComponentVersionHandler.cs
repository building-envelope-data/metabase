using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Marten;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Aggregates = Icon.Aggregates;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public class GetComponentVersionHandler
      : IQueryHandler<Queries.GetComponentVersion, Result<Models.ComponentVersion, IError>>
    {
        private readonly IAggregateRepository _repository;

        public GetComponentVersionHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Models.ComponentVersion, IError>> Handle(
            Queries.GetComponentVersion query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return
                  (await session
                   .Load<Aggregates.ComponentVersionAggregate>(
                     query.ComponentVersionId,
                     timestamp: query.Timestamp,
                     cancellationToken: cancellationToken
                     )
                  ).Map(a => a.ToModel());
            }
        }
    }
}