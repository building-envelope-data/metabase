using System;
using Marten;
using Marten.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Event;

namespace Icon.Infrastructure.Aggregate
{
    public interface IAggregateRepository
    {
        public Task<int> FetchVersion<T>(Guid streamId, DateTime timestamp, CancellationToken cancellationToken) where T : class, IEventSourcedAggregate, new();

        public Task<(Guid Id, DateTime Timestamp)> Store<T>(Guid streamId, int expectedVersion, IEvent @event, CancellationToken cancellationToken) where T : class, IEventSourcedAggregate, new();

        public Task<(Guid Id, DateTime Timestamp)> Store<T>(Guid streamId, int expectedVersion, IEnumerable<IEvent> events, CancellationToken cancellationToken) where T : class, IEventSourcedAggregate, new();

        public Task<T> Load<T>(Guid id, DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new();

        /* public Task<IEnumerable<T>> LoadAll<T>(DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new(); */

        public Task<IEnumerable<T>> LoadAll<T>(IEnumerable<Guid> ids, DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new();

        public IMartenQueryable<E> Query<E>() where E : IEvent;
    }
}