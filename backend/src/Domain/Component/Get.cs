using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Icon.Domain;
using Marten;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;

namespace Icon.Domain.Component.Get
{
    public class Query : IQuery<ComponentView>
    {
        public Guid ComponentId { get; set; }
        public DateTime Timestamp { get; set; } // TODO ZonedDateTime
    }

    public class QueryHandler :
        IQueryHandler<Query, ComponentView>
    {
        private readonly IDocumentSession _session;

        public QueryHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<ComponentView> Handle(Query query, CancellationToken cancellationToken)
        {
            // TODO Use `query.Timestamp` to load state at the specified point in time
            // This works for aggregates as explained in the section `Live Aggregation via .Net` on
            // https://jasperfx.github.io/marten/documentation/events/projections/
            // And it should work in general with something called `Live
            // Projections` which are mentioned on
            // https://jasperfx.github.io/marten/documentation/events/projections/
            return await _session.LoadAsync<ComponentView>(query.ComponentId, cancellationToken);
        }
    }
}