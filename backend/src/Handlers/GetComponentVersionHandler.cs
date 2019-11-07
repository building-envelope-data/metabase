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

namespace Icon.Handlers
{
    public class GetComponentVersionHandler
      : IQueryHandler<Queries.GetComponentVersion, Models.ComponentVersion>
    {
        private readonly IAggregateRepository _repository;

        public GetComponentVersionHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.ComponentVersion> Handle(
            Queries.GetComponentVersion query,
            CancellationToken cancellationToken
            )
        {
            return
              (await _repository
               .Load<Aggregates.ComponentVersionAggregate>(
                 query.ComponentVersionId,
                 timestamp: query.Timestamp,
                 cancellationToken: cancellationToken
                 )
              ).ToModel();
        }
    }
}