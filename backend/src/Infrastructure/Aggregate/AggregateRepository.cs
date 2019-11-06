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

        public async Task<T> Load<T>(Guid id, DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new()
        {
            var aggregate = await _session.Events.AggregateStreamAsync<T>(id, timestamp: timestamp, token: cancellationToken);
						if (aggregate == null || aggregate.Version == 0)
						{
							throw new InvalidOperationException($"There is no aggregate with id {id} at time {timestamp}.");
						}
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
								return
									(await Task.WhenAll(aggregateStreamTasks))
									  .Where(a => a.Version >= 1);
        }

				public IMartenQueryable<E> Query<E>() where E : IEvent
				{
                return _session.Events.QueryRawEventDataOnly<E>();
				}
    }
}