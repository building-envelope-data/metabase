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

        public async Task<T> Load<T>(Guid id, DateTime? timestamp = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new()
        {
            var aggregate = await _session.Events.AggregateStreamAsync<T>(id, timestamp: timestamp, token: cancellationToken);
            return aggregate ?? throw new InvalidOperationException($"There is no aggregate with id {id.ToString()}.");
        }

        public async Task<IEnumerable<T>> LoadAll<T>(DateTime? timestamp = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new()
        {
            if (timestamp == null)
            {
              // https://jasperfx.github.io/marten/documentation/documents/querying/async/
              return await _session.Query<T>()
                .ToListAsync(cancellationToken);
            }
            else
            {
              var aggregateIds = await _session.Query<T>()
                .Select(a => a.Id)
                .ToListAsync(cancellationToken);
              return await LoadAll<T>(aggregateIds, timestamp, cancellationToken);
            }
        }

        public async Task<IEnumerable<T>> LoadAll<T>(IEnumerable<Guid> ids, DateTime? timestamp = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new()
        {
          if (timestamp == null)
          {
              return await _session.Query<T>()
                .Where(a => ids.Contains(a.Id))
                .ToListAsync(cancellationToken);
          }
          else
          {
            var aggregates = new List<T>();
            foreach (var id in ids) {
              var aggregate = await _session.Events.AggregateStreamAsync<T>(id, timestamp: timestamp, token: cancellationToken);
              if (aggregate.Version >= 1)
              {
                aggregates.Add(aggregate);
              }
            }
            return aggregates.AsReadOnly();
            /* // The following does sadly not work because the various asynchronous database operations come in conflict with each other */
            /* var getAggregateTasks = new List<Task<T>>(); */
            /* foreach (var Id in allIds) { */
            /*   getAggregateTasks.Add( */
            /*       _session.Events */
            /*       .AggregateStreamAsync<T>( */
            /*         Id, */
            /*         timestamp: timestamp, */
            /*         token: cancellationToken */
            /*         ) */
            /*       ); */
            /* } */
            /* return */
            /*   (await Task.WhenAll(getAggregateTasks)) */
            /*   .Where(v => v.Version >= 1); */
          }
        }
    }
}