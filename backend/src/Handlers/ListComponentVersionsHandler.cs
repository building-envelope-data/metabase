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
using Marten.Linq.MatchesSql;
/* using Projections = Icon.Projections; */

namespace Icon.Handlers
{
    public sealed class ListComponentVersionsHandler :
      IQueryHandler<Queries.ListComponentVersions, IEnumerable<Models.ComponentVersion>>
    {
        private readonly IDocumentSession _session;
        private readonly IAggregateRepository _repository;

        public ListComponentVersionsHandler(IDocumentSession session, IAggregateRepository repository)
        {
            _session = session;
            _repository = repository;
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
                // TODO How can this be done efficiently? Live projections would be cleaner.

                /* _session.Events.QueryAllRawEvents() */
                /*   .Where(e => e.Timestamp <= query.Timestamp && */
                /*       e.Type == typeof(Events.ComponentVersionCreated)); */

                var allVersionIds = await _session.Events.QueryRawEventDataOnly<Events.ComponentVersionCreated>()
                  .Where(e => e.ComponentId == query.ComponentId)
                  .Select(e => e.ComponentVersionId)
                  .ToListAsync();

                /* // The stream id is equal to the component version id. */
                /* var versionIds = await _session.Events.QueryAllRawEvents() */
                /*   // .Where(e => allVersionIds.Contains(e.StreamId)) // Why does this not work? */
                /*   // .Where(e => e.MatchesSql("d.stream_id in ?", allVersionIds)) // Why does this not work? */
                /*   .Where(e => e.Timestamp <= query.Timestamp) */
                /*   .Select(e => e.StreamId) */
                /*   .ToListAsync(cancellationToken); */

                return
                  (await
                   _repository.LoadAll<Aggregates.ComponentVersionAggregate>(
                     allVersionIds,
                     query.Timestamp,
                     cancellationToken
                     )
                  )
                  .Select(a => a.ToModel());
            }
        }
    }
}