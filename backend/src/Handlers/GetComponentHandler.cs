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
    public class GetComponentHandler
      : IQueryHandler<Queries.GetComponent, Models.Component>
    {
        private readonly IAggregateRepository _repository;

        public GetComponentHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.Component> Handle(
            Queries.GetComponent query,
            CancellationToken cancellationToken
            )
        {
            return
              (await _repository
               .Load<Aggregates.ComponentAggregate>(
                 query.ComponentId,
                 timestamp: query.Timestamp,
                 cancellationToken: cancellationToken
                 )
              ).ToModel();
        }
    }
}