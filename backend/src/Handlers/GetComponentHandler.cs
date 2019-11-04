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

        public async Task<Models.Component> Handle(Queries.GetComponent query, CancellationToken cancellationToken)
        {
            // TODO Use `query.Timestamp` to load state at the specified point in time
            // This works for aggregates as explained in the section `Live Aggregation via .Net` on
            // https://jasperfx.github.io/marten/documentation/events/projections/
            // And it should work in general with something called `Live
            // Projections` which are mentioned on
            // https://jasperfx.github.io/marten/documentation/events/projections/
            return (await _session.LoadAsync<Aggregates.ComponentAggregate>(query.ComponentId, cancellationToken)).ToModel();
        }
    }
}
