// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;
using System;
using System.Linq;
using System.Collections.Generic;
using Marten;
using IEventStore = Marten.Events.IEventStore;
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

        public Task<Result<ValueObjects.TimestampedId, Errors>> New<T>(
            Guid id,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return New<T>(
                id,
                new IEvent[] { @event },
                cancellationToken
                );
        }

        public Task<Result<ValueObjects.TimestampedId, Errors>> New<T>(
            Guid id,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return Store<T>(
                id,
                events,
                (eventStore, eventArray) => eventStore.StartStream<T>(id, eventArray),
                cancellationToken
                );
        }

        public Task<Result<ValueObjects.TimestampedId, Errors>> Append<T>(
            Guid id,
            int expectedVersion,
            IEvent @event,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
            return Append<T>(
                id,
                expectedVersion,
                new IEvent[] { @event },
                cancellationToken
                );
        }

        public Task<Result<ValueObjects.TimestampedId, Errors>> Append<T>(
            Guid id,
            int expectedVersion,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return Store<T>(
                id,
                events,
                (eventStore, eventArray) => eventStore.Append(id, expectedVersion, eventArray),
                cancellationToken
                );
        }

        private async Task<Result<ValueObjects.TimestampedId, Errors>> Store<T>(
            Guid id,
            IEnumerable<IEvent> events,
            Action<IEventStore, IEvent[]> action,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            Event.EnsureValid(events);
            AssertExistenceOfCreators(
                events.Select(@event => @event.CreatorId),
                cancellationToken
                );
            var eventArray = events.ToArray();
            action(_session.Events, eventArray);
            await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await _eventBus.Publish(eventArray).ConfigureAwait(false);
            var timestampResult = await FetchTimestamp<T>(id, cancellationToken).ConfigureAwait(false);
            return ValueObjects.Id.From(id)
              .Bind(nonEmptyId =>
                  timestampResult.Bind(timestamp =>
                      ValueObjects.TimestampedId.From(
                        id, timestamp
                        )
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