using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Events = Icon.Events;
using Aggregates = Icon.Aggregates;
using System.Linq;
using Marten;
/* using Projections = Icon.Projections; */

namespace Icon.Handlers
{
    public sealed class ListComponentVersionsHandler :
      IQueryHandler<Queries.ListComponentVersions, IEnumerable<Models.ComponentVersion>>
    {
        private readonly IDocumentSession _session;

        public ListComponentVersionsHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Models.ComponentVersion>> Handle(
            Queries.ListComponentVersions query,
            CancellationToken cancellationToken
            )
        {
            if (query.Timestamp == null)
            {
                return
                  (await _session
                    .Query<Aggregates.ComponentVersionAggregate>()
                    .Where(a => a.ComponentId == query.ComponentId)
                    .ToListAsync(cancellationToken))
                  .Select(a => a.ToModel());
            }
            else
            {
                // TODO How can this be done (efficiently)?
                /* _session.Events.QueryAllRawEvents(); // Events.ComponentVersionCreated */
                /* new Projections.ComponentVersionsProjection().ApplyAsync(null, null, null); */
                return new List<Models.ComponentVersion>();
            }
        }
    }
}