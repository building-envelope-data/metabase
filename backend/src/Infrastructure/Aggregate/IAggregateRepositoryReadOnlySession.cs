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

        public Task<ValueObjects.Id> GenerateNewId(
            CancellationToken cancellationToken
            );

        public Task<bool> Exists(
            Guid id,
            CancellationToken cancellationToken
            );

        public Task<Result<int, Errors>> FetchVersion<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<ValueObjects.Timestamp, Errors>> FetchTimestamp<T>(
            Guid id,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<T, Errors>> Load<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<T, Errors>> Load<T>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<(Guid, DateTime)> possibleIdsAndTimestamps,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<Guid> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAllThatExisted<T>(
            IEnumerable<Guid> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAllThatExisted<T>(
            IEnumerable<ValueObjects.Id> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();
    }
}