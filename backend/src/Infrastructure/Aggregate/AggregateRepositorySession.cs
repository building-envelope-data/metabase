// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Linq;
using System.Collections.Generic;
using Marten;
using Marten.Linq;
using System.Threading.Tasks;
using Icon.Events;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Icon.ErrorCodes;
using HotChocolate;
using CSharpFunctionalExtensions;

namespace Icon.Infrastructure.Aggregate
{
    public sealed class AggregateRepositorySession : AggregateRepositoryReadOnlySession, IAggregateRepositorySession
    {
        private readonly IDocumentSession _session;
        private readonly IEventBus _eventBus;

        public AggregateRepositorySession(IDocumentSession session, IEventBus eventBus)
          : base(session)
        {
            _session = session;
            _eventBus = eventBus;
        }

        public Task<Result<(Guid Id, DateTime Timestamp), IError>> Store<T>(
            Guid id,
            int expectedVersion,
            IEvent @event,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return Store<T>(
                id,
                expectedVersion,
                new IEvent[] { @event },
                cancellationToken
                );
        }

        public async Task<Result<(Guid Id, DateTime Timestamp), IError>> Store<T>(
            Guid id,
            int expectedVersion,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            Event.EnsureValid(events);
            AssertExistenceOfCreators(
                events.Select(@event => @event.CreatorId),
                cancellationToken
                );
            var eventArray = events.ToArray();
            _session.Events.Append(id, expectedVersion, eventArray);
            await _session.SaveChangesAsync(cancellationToken);
            await _eventBus.Publish(eventArray);
            var timestampResult = await FetchTimestamp<T>(id, cancellationToken);
            return timestampResult.Map(
                timestamp => (
                  Id: id,
                  Timestamp: timestamp
                  )
                );
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
    }
}