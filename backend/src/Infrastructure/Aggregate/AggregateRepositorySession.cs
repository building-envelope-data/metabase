// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Icon.Events;
using Marten;
using Marten.Linq;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Icon.ErrorCodes;
using Errors = Icon.Errors;
using IEventStore = Marten.Events.IEventStore;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Infrastructure.Aggregate
{
    public sealed class AggregateRepositorySession : AggregateRepositoryReadOnlySession, IAggregateRepositorySession
    {
        private readonly IDocumentSession _session;
        private readonly IEventBus _eventBus;
        private IEnumerable<IEvent> _unsavedEvents;

        public AggregateRepositorySession(IDocumentSession session, IEventBus eventBus)
          : base(session)
        {
            _session = session;
            _eventBus = eventBus;
            _unsavedEvents = Enumerable.Empty<IEvent>();
        }

        public async Task<Result<ValueObjects.Id, Errors>> Create<T>(
            Func<Guid, Events.ICreatedEvent> newCreatedEvent,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
          var id = await GenerateNewId(cancellationToken).ConfigureAwait(false);
          return (
            await Create<T>(
                newCreatedEvent(id),
                cancellationToken
              )
            .ConfigureAwait(false)
            )
            .Map(_ => id);
        }

        public async Task<Result<bool, Errors>> Create<T>(
            ICreatedEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return
              await RegisterEvents(
                new IEvent[] { @event },
                eventArray => _session.Events.StartStream<T>(@event.AggregateId, eventArray),
                cancellationToken
                );
        }

        public Task<Result<bool, Errors>> Append<T>(
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

        public async Task<Result<bool, Errors>> Append<T>(
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
                    eventArray => _session.Events.Append(
                    timestampedId.Id, version + eventArray.Length, eventArray
                    ),
                    cancellationToken
                    )
                  .ConfigureAwait(false)
                )
              .ConfigureAwait(false);
        }

        public Task<Result<bool, Errors>> Delete<T>(
            ValueObjects.Timestamp timestamp,
            IDeletedEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return
              ValueObjects.TimestampedId
              .From(@event.AggregateId, timestamp)
              .Bind(timestampedId =>
                  {
                  _session.Delete<T>(timestampedId.Id);
                  return Append<T>(
                      timestampedId,
                      @event,
                      cancellationToken
                      );
                  }
                  );
        }

        public async Task<Result<bool, Errors>> Delete<T>(
            IEnumerable<(ValueObjects.Timestamp, IDeletedEvent)> timestampsAndEvents,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            var deletionResults = new List<Result<bool, Errors>>();
            foreach (var (timestamp, @event) in timestampsAndEvents)
            {
                deletionResults.Add(
                    await Delete<T>(
                      timestamp, @event, cancellationToken
                      )
                    .ConfigureAwait(false)
                    );
            }
            return Result.Combine<bool, Errors>(deletionResults);
        }

        public async Task<Result<bool, Errors>> Save(
            CancellationToken cancellationToken
            )
        {
            try
            {
                await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Marten.Services.EventStreamUnexpectedMaxEventIdException exception)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"The aggregate with identifier {exception.Id} of type {exception.AggregateType} was changed after the given timestamp causing the exception {exception}",
                      code: ErrorCodes.OutOfDate
                      )
                );
            }
            catch (Marten.Events.ExistingStreamIdCollisionException exception)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"The identifier {exception.Id} is already in use and can not be used for the new aggregate of type {exception.AggregateType} causing the exception {exception}",
                      code: ErrorCodes.IdCollision
                      )
                );
            }
            await _eventBus.Publish(_unsavedEvents).ConfigureAwait(false);
            _unsavedEvents = Enumerable.Empty<IEvent>();
            return
              await Task.FromResult<Result<bool, Errors>>(
                  Result.Ok<bool, Errors>(true)
                  )
              .ConfigureAwait(false);
        }

        private async Task<Result<bool, Errors>> RegisterEvents(
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
                )
              .ConfigureAwait(false);
            action(eventArray);
            _unsavedEvents = _unsavedEvents.Concat(eventArray);
            return
              await Task.FromResult<Result<bool, Errors>>(
                  Result.Ok<bool, Errors>(true)
                  )
              .ConfigureAwait(false);
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