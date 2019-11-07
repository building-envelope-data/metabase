// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Linq;
using System.Collections.Generic;
using Marten;
using Marten.Linq;
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

        public IMartenQueryable<E> Query<E>() where E : IEvent
        {
            return _session.Events.QueryRawEventDataOnly<E>();
        }

        public async Task<int> FetchVersion<T>(Guid streamId, DateTime timestamp, CancellationToken cancellationToken) where T : class, IEventSourcedAggregate, new()
        {
            // TODO For performance reasons it would be great if we could use (the parameter `timestamp` is not implemented though)
            // var expectedVersion = (await _session.Events.FetchStreamStateAsync(streamId, timestamp: timestamp, token: cancellationToken)).Version;
            // Ask on https://github.com/JasperFx/marten/issues for the parameter `timestamp` to be implemented
            return (await _session.Events.AggregateStreamAsync<T>(streamId, timestamp: timestamp, token: cancellationToken)).Version;
        }

        public Task<Guid> Store<E>(Guid streamId, int expectedVersion, E @event, CancellationToken cancellationToken) where E : IEvent
        {
            return Store(streamId, expectedVersion, new E[] { @event }, cancellationToken);
        }

        public async Task<Guid> Store<E>(Guid streamId, int expectedVersion, IEnumerable<E> events, CancellationToken cancellationToken) where E : IEvent
        {
            AssertExistenceOfCreators(
                                events.Select(@event => @event.CreatorId),
                                cancellationToken
                                );
            var eventArray = events.ToArray();
            _session.Events.Append(streamId, expectedVersion, eventArray);
            await _session.SaveChangesAsync(cancellationToken);
            await _eventBus.Publish(eventArray);
            return streamId;
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

        public async Task<T> Load<T>(Guid id, DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new()
        {
            var aggregate = await _session.Events.AggregateStreamAsync<T>(id, timestamp: timestamp, token: cancellationToken);
            if (aggregate == null || aggregate.Version == 0)
            {
                throw new InvalidOperationException($"There is no aggregate with id {id} at time {timestamp}.");
            }
            aggregate.Timestamp = timestamp;
            return aggregate;
        }

        /* public async Task<IEnumerable<T>> LoadAll<T>(DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new() */
        /* { */
        /*         var aggregateIds = await _session.Query<T>() */
        /*           .Select(a => a.Id) */
        /*           .ToListAsync(cancellationToken); */
        /*         return await LoadAll<T>(aggregateIds, timestamp, cancellationToken); */
        /* } */

        public async Task<IEnumerable<T>> LoadAll<T>(IEnumerable<Guid> possibleIds, DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new()
        {
            var batch = _session.CreateBatchQuery();
            var aggregateStreamTasks = new List<Task<T>>();
            foreach (var id in possibleIds)
            {
                aggregateStreamTasks.Add(
                                        batch.Events.AggregateStream<T>(id, timestamp: timestamp)
                                        );
            }
            await batch.Execute(cancellationToken);
            var aggregates =
                                (await Task.WhenAll(aggregateStreamTasks))
                                  .Where(a => a.Version >= 1);
            foreach (var aggregate in aggregates)
            {
                aggregate.Timestamp = timestamp;
            }
            return aggregates;
        }
    }
}