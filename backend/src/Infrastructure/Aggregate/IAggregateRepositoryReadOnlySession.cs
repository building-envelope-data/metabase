using System;
using Marten;
using Marten.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Events;
using HotChocolate;
using CSharpFunctionalExtensions;

namespace Icon.Infrastructure.Aggregate
{
    public interface IAggregateRepositoryReadOnlySession : IDisposable
    {
        public IMartenQueryable<E> Query<E>() where E : IEvent;

        public Task<Guid> GenerateNewId(
            CancellationToken cancellationToken
            );

        public Task<bool> Exists(
            Guid id,
            CancellationToken cancellationToken
            );

        public Task<Result<int, IError>> FetchVersion<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new();

        public Task<Result<DateTime, IError>> FetchTimestamp<T>(
            Guid id,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new();

        public Task<Result<T, IError>> Load<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, IError>>> LoadAll<T>(
            IEnumerable<(Guid Id, DateTime timestamp)> possibleIdsAndTimestamps,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, IError>>> LoadAll<T>(
            IEnumerable<Guid> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, IError>>> LoadAllThatExisted<T>(
            IEnumerable<Guid> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new();
    }
}