// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Linq;
using System.Collections.Generic;
using Marten;
using System.Threading.Tasks;
using Icon.Infrastructure.Event;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Infrastructure.Aggregate
{
    public sealed class AggregateRepository : IAggregateRepository
    {
        private readonly IDocumentSession _session;
        private readonly IEventBus _eventBus;

        public AggregateRepository(IDocumentSession session, IEventBus eventBus)
        {
            _session = session;
            _eventBus = eventBus;
        }

        public async Task<T> Store<T>(T aggregate, CancellationToken cancellationToken) where T : IEventSourcedAggregate
        {
            // Take non-persisted events, push them to the event stream, indexed by the aggregate ID
            var events = aggregate.GetUncommittedEvents().ToArray();
            AssertExistenceOfCreators(events.Select(@event => @event.CreatorId), cancellationToken);
            _session.Events.Append(aggregate.Id, aggregate.Version, events);
            await _session.SaveChangesAsync(cancellationToken);
            await _eventBus.Publish(events);
            return aggregate;
        }

        private void AssertExistenceOfCreators(IEnumerable<Guid> creatorIds, CancellationToken cancellationToken)
        {
            // TODO
            /* foreach (var creatorId in creatorIds) */
            /* { */
            /*   if (!_session.Query<UserAggregate>().AnyAsync(user => user.Id == creatorId, cancellationToken)) */
            /*     throw new ArgumentException("Creator does not exist!", nameof(creatorId)); */
            /* } */
        }

        public async Task<T> Load<T>(Guid id, int? version = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new()
        {
            var aggregate = await _session.Events.AggregateStreamAsync<T>(id, version ?? 0, token: cancellationToken);
            return aggregate ?? throw new InvalidOperationException($"No aggregate by id {id.ToString()}.");
        }

        public async Task<IEnumerable<T>> LoadAll<T>(CancellationToken cancellationToken) where T : IEventSourcedAggregate, new()
        {
            // https://jasperfx.github.io/marten/documentation/documents/querying/async/
            return await _session.Query<T>().ToListAsync(cancellationToken);
        }
    }
}