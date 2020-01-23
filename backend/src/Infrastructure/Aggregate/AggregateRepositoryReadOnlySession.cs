// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Linq;
using System.Collections.Generic;
using Marten;
using Marten.Linq;
using System.Threading.Tasks;
using Icon.Events;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Icon.ErrorCodes;
using HotChocolate;
using CSharpFunctionalExtensions;

namespace Icon.Infrastructure.Aggregate
{
    public class AggregateRepositoryReadOnlySession : IAggregateRepositoryReadOnlySession
    {
        private readonly IDocumentSession _session;
        private bool _disposed;

        // TODO We sould like to use `IQuerySession` here, which however does
        // not provide access to an `IEventStore` via a getter `Events`.
        public AggregateRepositoryReadOnlySession(IDocumentSession session)
        {
            _session = session;
            _disposed = false;
        }

        protected void AssertNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("This session has been disposed");
            }
        }

        // Inspired by https://github.com/JasperFx/marten/blob/master/src/Marten/QuerySession.cs
        // The recommendation documentation of how to implement the IDisposable interface is slightly different
        // https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.8#idisposable-and-the-inheritance-hierarchy
        ~AggregateRepositoryReadOnlySession()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _session.Dispose();
            GC.SuppressFinalize(this);
        }

        private Errors BuildNonExistentModelError(Guid id)
        {
            return
              Errors.One(
                  message: $"There is no model with id {id}",
                  code: ErrorCodes.NonExistentModel
                  );
        }

        public IMartenQueryable<E> Query<E>() where E : IEvent
        {
            AssertNotDisposed();
            return _session.Events.QueryRawEventDataOnly<E>();
        }

        public async Task<ValueObjects.Id> GenerateNewId(
            CancellationToken cancellationToken
            )
        {
            AssertNotDisposed();
            var id = ValueObjects.Id.New();
            while (await Exists(id, cancellationToken))
            {
                id = ValueObjects.Id.New();
            }
            return id;
        }

        public async Task<bool> Exists(
            Guid id,
            CancellationToken cancellationToken
            )
        {
            var streamState =
              await _session.Events.FetchStreamStateAsync(
                  id,
                  token: cancellationToken
                  );
            return streamState != null;
        }

        public async Task<Result<int, Errors>> FetchVersion<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
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
            if (aggregate is null)
            {
                return Result.Failure<int, Errors>(BuildNonExistentModelError(id));
            }
            return Result.Ok<int, Errors>(aggregate.Version);
        }

        public async Task<Result<ValueObjects.Timestamp, Errors>> FetchTimestamp<T>(
            Guid id,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var streamState =
              await _session.Events.FetchStreamStateAsync(
                  id,
                  token: cancellationToken
                  );
            if (streamState is null)
            {
                return Result.Failure<ValueObjects.Timestamp, Errors>(BuildNonExistentModelError(id));
            }
            return ValueObjects.Timestamp.From(streamState.LastTimestamp);
        }

        public async Task<Result<T, Errors>> Load<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var aggregate =
              await _session.Events.AggregateStreamAsync<T>(
                  id,
                  timestamp: timestamp,
                  token: cancellationToken
                  );
            return BuildResult(id, aggregate);
        }

        public Task<Result<T, Errors>> Load<T>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return Load<T>(
                timestampedId.Id,
                timestampedId.Timestamp,
                cancellationToken
                );
        }

        /* public async Task<IEnumerable<T>> LoadAll<T>(DateTime timestamp, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEventSourcedAggregate, new() */
        /* { */
        /*         var aggregateIds = await _session.Query<T>() */
        /*           .Select(a => a.Id) */
        /*           .ToListAsync(cancellationToken); */
        /*         return await LoadAll<T>(aggregateIds, timestamp, cancellationToken); */
        /* } */

        public async Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<(Guid, DateTime)> idsAndTimestamps,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var batch = _session.CreateBatchQuery();
            var aggregateStreamTasks =
              idsAndTimestamps
              .Select(((Guid id, DateTime timestamp) t) // There sadly is no proper tuple deconstruction in lambdas yet. For details see https://github.com/dotnet/csharplang/issues/258
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

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return LoadAll<T>(
                timestampedIds.Select(x => ((Guid, DateTime))x),
                cancellationToken
                );
        }

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<Guid> ids,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return LoadAll<T>(
                ids.Select(id => (Id: id, Timestamp: timestamp)),
                cancellationToken
                );
        }

        public async Task<IEnumerable<Result<T, Errors>>> LoadAllThatExisted<T>(
            IEnumerable<Guid> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return
              (await LoadAll<T>(
                                possibleIds.Select(id => (Id: id, Timestamp: timestamp)),
                                cancellationToken
                               )
              ).Where(r =>
                // TODO This way of excluding errors is error-prone and is also
                // somewhat not what we want. We, for example, exclude an error
                // right now if the model has never existed neither in the past
                // nor in the future. This should however still be reported as
                // an error. We only want to exclude the models that have been
                // created after the respective timestamp. This information
                // however cannot be extracted from the error right now. There
                // are two solutions: Either differentiate between those two
                // errors, or extract the common logic from `LoadAll` that is
                // also needed by this method into a helper method and use that
                // helper here. I would prefere the latter method because it's
                // less error-prone.
                r.IsSuccess ||
                (
                 r.IsFailure &&
                 (
                  r.Error.Count != 1 ||
                  r.Error[0].Code != ErrorCodes.NonExistentModel
                 )
                )
                );
        }

        public Task<IEnumerable<Result<T, Errors>>> LoadAllThatExisted<T>(
            IEnumerable<ValueObjects.Id> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return LoadAllThatExisted<T>(
                possibleIds.Select(x => (Guid)x),
                timestamp,
                cancellationToken
                );
        }

        private Result<T, Errors> BuildResult<T>((Guid Id, DateTime Timestamp) idAndTimestamp, T aggregate)
          where T : class, IEventSourcedAggregate, new()
        {
            return BuildResult(idAndTimestamp.Id, aggregate);
        }

        private Result<T, Errors> BuildResult<T>(Guid id, T aggregate)
          where T : class, IEventSourcedAggregate, new()
        {
            aggregate?.EnsureValid();
            if (aggregate is null || aggregate.Version == 0)
            {
                return Result.Failure<T, Errors>(BuildNonExistentModelError(id));
            }
            return Result.Ok<T, Errors>(aggregate);
        }
    }
}