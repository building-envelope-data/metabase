using System;
using Marten;
using System.Threading.Tasks;
using System.Collections.Generic;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Infrastructure.Aggregate
{
    public interface IAggregateRepository
    {
        public Task<T> Store<T>(T aggregate, CancellationToken cancellationToken = default(CancellationToken)) where T : IEventSourcedAggregate;

        public Task<T> Load<T>(Guid id, DateTime? timestamp = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<T>> LoadAll<T>(DateTime? timestamp = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<T>> LoadAll<T>(IEnumerable<Guid> ids, DateTime? timestamp = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new();
    }
}