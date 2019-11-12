// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Linq;
using System.Collections.Generic;
using Marten;
using Marten.Linq;
using System.Threading.Tasks;
using Icon.Infrastructure.Event;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Icon.ErrorCodes;
using HotChocolate;
using CSharpFunctionalExtensions;

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

        private IError BuildNonExistentModelError(Guid id) =>
          ErrorBuilder.New()
          .SetMessage($"There is no model with id {id}.")
          .SetCode(ErrorCodes.NonExistentModel)
          .Build();

        public IMartenQueryable<E> Query<E>() where E : IEvent
        {
          return _session.Events.QueryRawEventDataOnly<E>();
        }

        public async Task<Result<int, IError>> FetchVersion<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
          // TODO For performance reasons it would be great if we could use
          // var expectedVersion = (await _session.Events.FetchStreamStateAsync(id, timestamp: timestamp, token: cancellationToken)).Version;
          // (the parameter `timestamp` is not implemented though)
          // Ask on https://github.com/JasperFx/marten/issues for the parameter `timestamp` to be implemented
          var aggregate =
            await _session.Events.AggregateStreamAsync<T>(
                id,
                timestamp: timestamp,
                token: cancellationToken
                );
          if (aggregate == null) {
            return Result.Failure<int, IError>(BuildNonExistentModelError(id));
          }
          return Result.Success<int, IError>(aggregate.Version);
        }

        public async Task<Result<DateTime, IError>> FetchTimestamp<T>(
            Guid id,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
          var streamState =
            await _session.Events.FetchStreamStateAsync(
                id,
                token: cancellationToken
                );
          if (streamState == null) {
            return Result.Failure<DateTime, IError>(BuildNonExistentModelError(id));
          }
          return Result.Success<DateTime, IError>(streamState.LastTimestamp);
        }

        public Task<Result<(Guid Id, DateTime Timestamp), IError>> Store<T>(
            Guid id,
            int expectedVersion,
            IEvent @event,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
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

        public async Task<Result<T, IError>> Load<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new()
        {
          var aggregate =
            await _session.Events.AggregateStreamAsync<T>(
                id,
                timestamp: timestamp,
                token: cancellationToken
                );
          return BuildResult(id, timestamp, aggregate);
        }

        /* public async Task<IEnumerable<T>> LoadAll<T>(DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new() */
        /* { */
        /*         var aggregateIds = await _session.Query<T>() */
        /*           .Select(a => a.Id) */
        /*           .ToListAsync(cancellationToken); */
        /*         return await LoadAll<T>(aggregateIds, timestamp, cancellationToken); */
        /* } */

        public async Task<IEnumerable<Result<T, IError>>> LoadAll<T>(
            IEnumerable<(Guid Id, DateTime timestamp)> idsAndTimestamps,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new()
        {
          var batch = _session.CreateBatchQuery();
          var aggregateStreamTasks =
            idsAndTimestamps
            .Select(
                // There sadly is no proper tuple deconstruction in lambdas yet. For details see https://github.com/dotnet/csharplang/issues/258
                ((Guid id, DateTime timestamp) t)
                => batch.Events.AggregateStream<T>(t.id, timestamp: t.timestamp)
                )
            // Turning the `System.Linq.Enumerable+SelectListIterator` into a list is necessary for `await Task.WhenAll(aggregateStreamTasks) to finish`
            .ToList();
          await batch.Execute(cancellationToken);
          var aggregates = await Task.WhenAll(aggregateStreamTasks);
          return
            idsAndTimestamps
            .Zip(aggregates, BuildResult);
        }

        public Task<IEnumerable<Result<T, IError>>> LoadAll<T>(
            IEnumerable<Guid> ids,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new()
        {
          return LoadAll<T>(
              ids.Select(id => (Id: id, Timestamp: timestamp)),
              cancellationToken
              );
        }

        public async Task<IEnumerable<Result<T, IError>>> LoadAllThatExisted<T>(
            IEnumerable<Guid> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new()
        {
           return
             (await LoadAll<T>(
                              possibleIds.Select(id => (Id: id, Timestamp: timestamp)),
                              cancellationToken
                             )
            ).Where(r => r.IsFailure || r.Value.Version >= 1);
        }

        private Result<T, IError> BuildResult<T>((Guid Id, DateTime Timestamp) idAndTimestamp, T aggregate)
          where T : class, IEventSourcedAggregate, new()
        {
            return BuildResult(idAndTimestamp.Id, idAndTimestamp.Timestamp, aggregate);
        }

        private Result<T, IError> BuildResult<T>(Guid id, DateTime timestamp, T aggregate)
          where T : class, IEventSourcedAggregate, new()
          {
            if (aggregate == null || aggregate.Version == 0)
            {
              return Result.Failure<T, IError>(BuildNonExistentModelError(id));
            }
            aggregate.Timestamp = timestamp; // TODO This is a problem when aggregates are cached!
            return Result.Success<T, IError>(aggregate);
          }
    }
}