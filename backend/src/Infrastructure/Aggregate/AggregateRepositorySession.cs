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
            ValueObjects.TimestampedId timestampedId,
            IEvent @event,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
            return Append<T>(
                timestampedId,
                new IEvent[] { @event },
                cancellationToken
                );
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Append<T>(
            ValueObjects.TimestampedId timestampedId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            var expectedVersion = await FetchVersion<T>(timestampedId, cancellationToken);
            return await Store<T>(
                timestampedId.Id,
                events,
                (eventStore, eventArray) => eventStore.Append(timestampedId.Id, expectedVersion, eventArray),
                cancellationToken
                );
        }

        public Task<Result<ValueObjects.TimestampedId, Errors>> Delete<T>(
            ValueObjects.TimestampedId timestampedId,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            _session.Delete<T>(timestampedId.Id);
            return Append<T>(
                timestampedId,
                @event,
                cancellationToken
                );
        }

        public Task<Result<ValueObjects.TimestampedId, Errors>> Delete<T>(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return
              ValueObjects.TimestampedId.From(id, timestamp)
              .Bind(timestampedId =>
                Delete<T>(
                  timestampedId,
                  @event,
                  cancellationToken
                  )
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
            return await TimestampId<T>(id, cancellationToken);
        }

        private void AssertExistenceOfCreators(IEnumerable<Guid> creatorIds, CancellationToken cancellationToken)
        {
            /* foreach (var creatorId in creatorIds) */
            /* { */
            /*   if (!_session.Query<UserAggregate>().AnyAsync(user => user.Id == creatorId, cancellationToken)) */
            /*     throw new ArgumentException("Creator does not exist!", nameof(creatorId)); */
            /* } */
        }
    }
}