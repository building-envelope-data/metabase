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
        public Task<T> Store<T>(T aggregate, CancellationToken cancellationToken = default(CancellationToken)) where T : IEventSourcedAggregate;

        public Task<T> Load<T>(Guid id, DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new();

        /* public Task<IEnumerable<T>> LoadAll<T>(DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new(); */

        public Task<IEnumerable<T>> LoadAll<T>(IEnumerable<Guid> ids, DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new();

				public IMartenQueryable<E> Query<E>() where E : IEvent;
    }
}