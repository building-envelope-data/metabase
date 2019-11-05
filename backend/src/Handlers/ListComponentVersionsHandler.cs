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
                /*   // .Where(e => allVersionIds.Contains(e.StreamId)) // TODO Why does this not work? */
                /*   // .Where(e => e.MatchesSql("d.stream_id in ?", allVersionIds)) // TODO Why does this not work? */
                /*   .Where(e => e.Timestamp <= query.Timestamp) */
                /*   .Select(e => e.StreamId) */
                /*   .ToListAsync(cancellationToken); */

                // The following does sadly not work because the various asynchronous database operations come in conflict with each other
                /* var getAggregateTasks = new List<Task<Aggregates.ComponentVersionAggregate>>(); */
                /* foreach (var versionId in allVersionIds) { */
                /*   getAggregateTasks.Add( */
                /*       _session.Events */
                /*        .AggregateStreamAsync<Aggregates.ComponentVersionAggregate>( */
                /*          versionId, */
                /*          timestamp: query.Timestamp, */
                /*          token: cancellationToken */
                /*          ) */
                /*       ); */
                /* } */
                /* return */
                /*   (await Task.WhenAll(getAggregateTasks)) */
                /*   .Where(v => v.Version >= 1) */
                /*   .Select(v => v.ToModel()); */

                var versions = new List<Models.ComponentVersion>();
                foreach (var versionId in allVersionIds) {
                  var aggregate =
                      await _session.Events
                       .AggregateStreamAsync<Aggregates.ComponentVersionAggregate>(
                         versionId,
                         timestamp: query.Timestamp,
                         token: cancellationToken
                         );
                  if (aggregate.Version >= 1)
                    versions.Add(aggregate.ToModel());
                }
                return versions;
            }
        }
    }
}