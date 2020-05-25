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
        public IMartenQueryable<E> QueryEvents<E>() where E : IEvent;

        public IMartenQueryable<T> Query<T>() where T : class, IEventSourcedAggregate, new();

        public Task<ValueObjects.Id> GenerateNewId(
            CancellationToken cancellationToken
            );

        public Task<bool> Exists(
            Guid id,
            CancellationToken cancellationToken
            );

        public Task<bool> Exists<T>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<bool>> Exist<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<int, Errors>> FetchVersion<T>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<ValueObjects.Timestamp, Errors>> FetchTimestamp<T>(
            Guid id,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<Type, Errors>> FetchAggregateType(
            Guid id,
            CancellationToken cancellationToken
            );

        public Task<IEnumerable<Result<Type, Errors>>> FetchAggregateTypes(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken
            );

        public Task<IEnumerable<Result<Type, Errors>>> FetchAggregateTypes(
            IEnumerable<ValueObjects.Id> ids,
            CancellationToken cancellationToken
            );

        public Task<Result<ValueObjects.TimestampedId, Errors>> TimestampId<T>(
            Guid id,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<T, Errors>> Load<T>(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
                    where T : class, IEventSourcedAggregate, new();

        public Task<Result<T, Errors>> Load<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<T, Errors>> LoadX<T>(
            Guid id,
            DateTime timestamp,
            Func<IAggregateRepositoryReadOnlySession, Type, Guid, DateTime, CancellationToken, Task<Result<T, Errors>>> load,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        public Task<Result<T, Errors>> Load<T>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<(Guid, DateTime)> idsAndTimestamps,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAllX<T>(
            IEnumerable<(Guid, DateTime)> idsAndTimestamps,
            Func<IAggregateRepositoryReadOnlySession, Type, IEnumerable<(Guid, DateTime)>, CancellationToken, Task<IEnumerable<Result<T, Errors>>>> loadAll,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<Guid> ids,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAllX<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            Func<IAggregateRepositoryReadOnlySession, Type, IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<T, Errors>>>> loadAll,
            CancellationToken cancellationToken = default(CancellationToken)
            );

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

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllBatched<T>(
            IEnumerable<(DateTime, IEnumerable<Guid>)> timestampsAndIds,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllBatched<T>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndIds,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllThatExistedBatched<T>(
            IEnumerable<(DateTime, IEnumerable<Guid>)> timestampsAndPossibleIds,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllThatExistedBatched<T>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndPossibleIds,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new();
    }
}