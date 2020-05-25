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
        private List<IEvent[]> _unsavedEvents;

        public AggregateRepositorySession(IDocumentSession session, IEventBus eventBus)
          : base(session)
        {
            _session = session;
            _eventBus = eventBus;
            _unsavedEvents = new List<IEvent[]>();
        }

        public Task<Result<IAggregateRepositorySession, Errors>> Create<T>(
            Guid id,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return Create<T>(
                id,
                new IEvent[] { @event },
                cancellationToken
                );
        }

        public Task<Result<IAggregateRepositorySession, Errors>> Create<T>(
            Guid id,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return RegisterEvents(
                events,
                eventArray => _session.Events.StartStream<T>(id, eventArray),
                cancellationToken
                );
        }

        public Task<Result<IAggregateRepositorySession, Errors>> Append<T>(
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

        public async Task<Result<IAggregateRepositorySession, Errors>> Append<T>(
            ValueObjects.TimestampedId timestampedId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return await (
                await FetchVersion<T>(
                  timestampedId,
                  cancellationToken
                  )
               .ConfigureAwait(false)
               )
              .Bind(async version => await
                  RegisterEvents(
                    events,
                    eventArray => _session.Events.Append(timestampedId.Id, version + 1, eventArray),
                    cancellationToken
                    )
                  .ConfigureAwait(false)
                );
        }

        public Task<Result<IAggregateRepositorySession, Errors>> Delete<T>(
            ValueObjects.TimestampedId timestampedId,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            _session.Delete<T>(timestampedId.Id);
            return Append<T>(
                timestampedId,
                @event,
                cancellationToken
                );
        }

        public Task<Result<IAggregateRepositorySession, Errors>> Delete<T>(
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

        public async Task<Result<IAggregateRepositorySession, Errors>> Save(
            CancellationToken cancellationToken
            )
        {
            await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            foreach (var events in _unsavedEvents)
            {
                await _eventBus.Publish(events).ConfigureAwait(false);
            }
            _unsavedEvents = new List<IEvent[]>();
            return await Task.FromResult<Result<IAggregateRepositorySession, Errors>>(
                Result.Ok<IAggregateRepositorySession, Errors>(this)
                );
        }

        private async Task<Result<IAggregateRepositorySession, Errors>> RegisterEvents(
            IEnumerable<IEvent> events,
            Action<IEvent[]> action,
            CancellationToken cancellationToken
            )
        {
            var eventArray = events.ToArray();
            Event.EnsureValid(eventArray);
            await AssertExistenceOfCreators(
                eventArray.Select(@event => @event.CreatorId),
                cancellationToken
                );
            action(eventArray);
            _unsavedEvents.Add(eventArray);
            return await Task.FromResult<Result<IAggregateRepositorySession, Errors>>(
                Result.Ok<IAggregateRepositorySession, Errors>(this)
                );
        }

        private Task AssertExistenceOfCreators(
            IEnumerable<Guid> creatorIds,
            CancellationToken cancellationToken
            )
        {
            return Task.CompletedTask;
            /* foreach (var creatorId in creatorIds) */
            /* { */
            /*   if (!_session.Query<UserAggregate>().AnyAsync(user => user.Id == creatorId, cancellationToken)) */
            /*     throw new ArgumentException("Creator does not exist!", nameof(creatorId)); */
            /* } */
        }
    }
}