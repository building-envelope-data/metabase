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
        private readonly IDocumentSession _session;

        public GetComponentHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<Models.Component> Handle(
            Queries.GetComponent query,
            CancellationToken cancellationToken
            )
        {
            if (query.Timestamp == null)
            {
                return
                  (await _session
                   .LoadAsync<Aggregates.ComponentAggregate>(
                     query.ComponentId,
                     token: cancellationToken
                     )
                  ).ToModel();
            }
            else
            {
                return
                  (await _session.Events
                   .AggregateStreamAsync<Aggregates.ComponentAggregate>(
                     query.ComponentId,
                     timestamp: query.Timestamp,
                     token: cancellationToken
                     )
                  ).ToModel();
            }
        }
    }
}