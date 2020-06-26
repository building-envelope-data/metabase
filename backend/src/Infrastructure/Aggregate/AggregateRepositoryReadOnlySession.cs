// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis; // NotNullWhen
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Icon.Events;
using Marten;
using Marten.Linq;
using CancellationToken = System.Threading.CancellationToken;
using StreamState = Marten.Events.StreamState;

namespace Icon.Infrastructure.Aggregate
{
    public class AggregateRepositoryReadOnlySession : IAggregateRepositoryReadOnlySession
    {
        private readonly IDocumentSession _session;
        private bool _disposed;

        // TODO We would like to use `IQuerySession` here, which however does
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

        public IMartenQueryable<E> QueryEvents<E>() where E : IEvent
        {
            AssertNotDisposed();
            return _session.Events.QueryRawEventDataOnly<E>();
        }

        public IMartenQueryable<T> Query<T>() where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return _session.Query<T>();
        }

        public async Task<ValueObjects.Id> GenerateNewId(
            CancellationToken cancellationToken
            )
        {
            AssertNotDisposed();
            var id = ValueObjects.Id.New();
            while (await Exists(id, cancellationToken).ConfigureAwait(false))
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
            AssertNotDisposed();
            var streamState =
              await _session.Events.FetchStreamStateAsync(
                  id,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            return streamState != null;
        }

        public async Task<bool> Exists<T>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var streamState =
              await _session.Events.FetchStreamStateAsync(
                  timestampedId.Id,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            return DoesStreamStateExist<T>(streamState, timestampedId.Timestamp);
        }

        public async Task<IEnumerable<bool>> Exist<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var batch = _session.CreateBatchQuery();
            var streamStateTasks =
              timestampedIds.Select(timestampedId =>
                batch.Events.FetchStreamState(timestampedId.Id)
                )
              .ToList(); // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var streamStates = await Task.WhenAll(streamStateTasks).ConfigureAwait(false);
            return timestampedIds.Zip(streamStates, (timestampedId, streamState) =>
                DoesStreamStateExist<T>(streamState, timestampedId.Timestamp)
                );
        }

        public async Task<Result<int, Errors>> FetchVersion<T>(
            ValueObjects.TimestampedId timestampedId,
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
                  timestampedId.Id,
                  timestamp: ((DateTime)timestampedId.Timestamp).ToUniversalTime(),
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            if (aggregate is null)
            {
                return Result.Failure<int, Errors>(
                    BuildNonExistentModelError(timestampedId.Id)
                    );
            }
            return Result.Ok<int, Errors>(aggregate.Version);
        }

        public async Task<Result<ValueObjects.Timestamp, Errors>> FetchTimestamp<T>(
            Guid id,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
            return (await FetchStreamState(id, cancellationToken).ConfigureAwait(false))
              .Bind(streamState =>
                  {
                      if (streamState.AggregateType != typeof(T))
                      {
                          return Result.Failure<ValueObjects.Timestamp, Errors>(
                              Errors.One(
                                message: $"The aggregate with id {id} is of type {streamState.AggregateType} and not of the expected type {typeof(T)}",
                                code: ErrorCodes.InvalidType
                                )
                              );
                      }
                      return ValueObjects.Timestamp.From(streamState.LastTimestamp);
                  }
                  );
        }

        public async Task<Result<Type, Errors>> FetchAggregateType(
            Guid id,
            CancellationToken cancellationToken
            )
        {
            return (await FetchStreamState(id, cancellationToken).ConfigureAwait(false))
              .Map(streamState => streamState.AggregateType);
        }

        public async Task<IEnumerable<Result<Type, Errors>>> FetchAggregateTypes(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken
            )
        {
            return (await FetchStreamStates(ids, cancellationToken).ConfigureAwait(false))
              .Select(streamStateResult =>
                  streamStateResult.Map(streamState =>
                    streamState.AggregateType
                    )
                  );
        }

        public Task<IEnumerable<Result<Type, Errors>>> FetchAggregateTypes(
            IEnumerable<ValueObjects.Id> ids,
            CancellationToken cancellationToken
            )
        {
            return FetchAggregateTypes(
                ids.Select(id => (Guid)id),
                cancellationToken
                );
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> TimestampId<T>(
            Guid id,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            var timestampResult = await FetchTimestamp<T>(id, cancellationToken).ConfigureAwait(false);
            return ValueObjects.Id.From(id)
              .Bind(nonEmptyId =>
                  timestampResult.Bind(timestamp =>
                    ValueObjects.TimestampedId.From(
                      id, timestamp
                      )
                    )
                  );
        }

        private async Task<Result<StreamState, Errors>> FetchStreamState(
            Guid id,
            CancellationToken cancellationToken
            )
        {
            AssertNotDisposed();
            var streamState =
              await _session.Events.FetchStreamStateAsync(
                  id,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            if (streamState is null)
            {
                return Result.Failure<StreamState, Errors>(BuildNonExistentModelError(id));
            }
            return Result.Ok<StreamState, Errors>(streamState);
        }

        private async Task<IEnumerable<Result<StreamState, Errors>>> FetchStreamStates(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken
            )
        {
            AssertNotDisposed();
            var batch = _session.CreateBatchQuery();
            var streamStateTasks =
              ids.Select(id =>
                  batch.Events.FetchStreamState(id)
                  )
              .ToList(); // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var streamStates = await Task.WhenAll(streamStateTasks).ConfigureAwait(false);
            return ids.Zip(streamStates, (id, streamState) =>
                  streamState is null
                    ? Result.Failure<StreamState, Errors>(BuildNonExistentModelError(id))
                    : Result.Ok<StreamState, Errors>(streamState)
                  );
        }

        public async Task<Result<T, Errors>> Load<T>(
            Guid id,
            CancellationToken cancellationToken = default
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            // Loading the materialized aggregate as follows
            // var aggregate = await _session.LoadAsync<T>(id, cancellationToken);
            // is not possible because event meta data is not available during
            // inline projection as said on
            // https://martendb.io/documentation/events/projections/
            // in the section on inline projections.
            // Therefore, version and timestamp of snapshots are not set.
            var aggregate =
              await _session.Events.AggregateStreamAsync<T>(
                  id,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            return BuildResult(id, aggregate);
        }

        public async Task<(Result<T1, Errors>, Result<T2, Errors>)> Load<T1, T2>(
            (Guid, Guid) ids,
            CancellationToken cancellationToken = default
            )
          where T1 : class, IEventSourcedAggregate, new()
          where T2 : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var batch = _session.CreateBatchQuery();
            // Loading the materialized aggregates as follows
            // batch.Load<T>(id);
            // is not possible because event meta data is not available during
            // inline projection as said on
            // https://martendb.io/documentation/events/projections/
            // in the section on inline projections.
            // Therefore, version and timestamp of snapshots are not set.
            var aggregateStreamTasks = (
              batch.Events.AggregateStream<T1>(ids.Item1),
              batch.Events.AggregateStream<T2>(ids.Item2)
              );
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            await Task.WhenAll(
                aggregateStreamTasks.Item1,
                aggregateStreamTasks.Item2
                )
              .ConfigureAwait(false);
            return (
                BuildResult(ids.Item1, aggregateStreamTasks.Item1.Result),
                BuildResult(ids.Item2, aggregateStreamTasks.Item2.Result)
                );
        }

        public async Task<Result<T, Errors>> Load<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken = default
            ) where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var aggregate =
              await _session.Events.AggregateStreamAsync<T>(
                  id,
                  timestamp: timestamp.ToUniversalTime(),
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            return BuildResult(id, aggregate);
        }

        public async Task<Result<T, Errors>> LoadX<T>(
            Guid id,
            DateTime timestamp,
            Func<IAggregateRepositoryReadOnlySession, Type, Guid, DateTime, CancellationToken, Task<Result<T, Errors>>> load,
            CancellationToken cancellationToken = default
            )
        {
            AssertNotDisposed();
            var aggregateTypeResult = await FetchAggregateType(id, cancellationToken).ConfigureAwait(false);
            return await aggregateTypeResult.Bind(async aggregateType =>
                await load(this, aggregateType, id, timestamp, cancellationToken).ConfigureAwait(false)
                )
              .ConfigureAwait(false);
        }

        public Task<Result<T, Errors>> Load<T>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return Load<T>(
                timestampedId.Id,
                timestampedId.Timestamp,
                cancellationToken
                );
        }

        public async Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var batch = _session.CreateBatchQuery();
            // Loading the materialized aggregates as follows
            // batch.Load<T>(id);
            // is not possible because event meta data is not available during
            // inline projection as said on
            // https://martendb.io/documentation/events/projections/
            // in the section on inline projections.
            // Therefore, version and timestamp of snapshots are not set.
            var aggregateStreamTasks =
              ids.Select(id => batch.Events.AggregateStream<T>(id))
              // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
              .ToList();
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var aggregates = await Task.WhenAll(aggregateStreamTasks).ConfigureAwait(false);
            return ids.Zip(aggregates, BuildResult);
        }

        /* public async Task<IEnumerable<T>> LoadAll<T>(DateTime timestamp, CancellationToken cancellationToken = default) where T : class, IEventSourcedAggregate, new() */
        /* { */
        /*         var aggregateIds = await _session.Query<T>() */
        /*           .Select(a => a.Id) */
        /*           .ToListAsync(cancellationToken); */
        /*         return await LoadAll<T>(aggregateIds, timestamp, cancellationToken); */
        /* } */

        private async Task<IEnumerable<(ValueObjects.Id, T?)>> LoadAllRaw<T>(
            IEnumerable<ValueObjects.Id> ids,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var batch = _session.CreateBatchQuery();
            var aggregateStreamTasks =
              ids.Select(id => batch.Events.AggregateStream<T>(id))
              // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
              .ToList();
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var aggregates = await Task.WhenAll(aggregateStreamTasks).ConfigureAwait(false);
            return ids.Zip(aggregates,
                (id, aggregate) => (id, (T?)aggregate)
                );
        }

        private async Task<IEnumerable<((Guid, DateTime), T?)>> LoadAllRaw<T>(
            IEnumerable<(Guid, DateTime)> idsAndTimestamps,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var batch = _session.CreateBatchQuery();
            var aggregateStreamTasks =
              idsAndTimestamps
              .Select(((Guid id, DateTime timestamp) t) // There sadly is no proper tuple deconstruction in lambdas yet. For details see https://github.com/dotnet/csharplang/issues/258
                  => batch.Events.AggregateStream<T>(t.id, timestamp: t.timestamp.ToUniversalTime())
                  )
              // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
              .ToList();
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var aggregates = await Task.WhenAll(aggregateStreamTasks).ConfigureAwait(false);
            return idsAndTimestamps.Zip(aggregates,
                (idAndTimestamp, aggregate) => (idAndTimestamp, (T?)aggregate)
                );
        }

        public async Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<(Guid, DateTime)> idsAndTimestamps,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return (await LoadAllRaw<T>(
                  idsAndTimestamps,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .Select((((Guid id, DateTime timestamp) idAndTimestamp, T? aggregate) t) =>
                  BuildResult(t.idAndTimestamp.id, t.aggregate)
                  );
        }

        public async Task<IEnumerable<Result<T, Errors>>> LoadAllX<T>(
            IEnumerable<(Guid, DateTime)> idsAndTimestamps,
            Func<IAggregateRepositoryReadOnlySession, Type, IEnumerable<(Guid, DateTime)>, CancellationToken, Task<IEnumerable<Result<T, Errors>>>> loadAll,
            CancellationToken cancellationToken = default
            )
        {
            AssertNotDisposed();
            var aggregateTypeResults = await FetchAggregateTypes(
                idsAndTimestamps.Select(((Guid id, DateTime timestamp) t) => t.id),
                cancellationToken
                )
              .ConfigureAwait(false);
            var aggregateTypeToIdsAndTimestamps =
              idsAndTimestamps.Zip(aggregateTypeResults)
              .Where(t => t.Second.IsSuccess)
              .ToLookup(
                  t => t.Second.Value,
                  t => t.First
                  );
            var aggregateTypes = aggregateTypeToIdsAndTimestamps.Select(g => g.Key);
            var results =
              await Task.WhenAll(
                  aggregateTypes.Select(aggregateType =>
                    loadAll(this, aggregateType, aggregateTypeToIdsAndTimestamps[aggregateType], cancellationToken)
                    )
                  )
              .ConfigureAwait(false);
            var aggregateTypeToResultsEnumerator =
              aggregateTypes.Zip(results).ToDictionary(
                  t => t.First,
                  t => t.Second.GetEnumerator()
                  );
            return aggregateTypeResults.Select(aggregateTypeResult =>
                    aggregateTypeResult.Bind(aggregateType =>
                      {
                          var enumerator = aggregateTypeToResultsEnumerator[aggregateType];
                          if (enumerator.MoveNext())
                          {
                              return enumerator.Current;
                          }
                          return Result.Failure<T, Errors>(
                                  Errors.One(
                                    message: $"There is no more result of aggregate type {aggregateType}",
                                    code: ErrorCodes.NonExistentModel
                                    )
                                  );
                      }
                        )
                );
        }

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return LoadAll<T>(
                timestampedIds.Select(x => ((Guid, DateTime))x),
                cancellationToken
                );
        }

        public Task<IEnumerable<Result<T, Errors>>> LoadAllX<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            Func<IAggregateRepositoryReadOnlySession, Type, IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<T, Errors>>>> loadAll,
            CancellationToken cancellationToken = default
            )
        {
            // TODO Avoid the unsafe cast of `t` to `ValueObjects.TimestampedId`
            return LoadAllX<T>(
                timestampedIds.Select(x => ((Guid, DateTime))x),
                (session, aggregateType, idsAndTimestamps, cancellationToken) =>
                  loadAll(session, aggregateType, idsAndTimestamps.Select(t => (ValueObjects.TimestampedId)t), cancellationToken),
                cancellationToken
                );
        }

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<Guid> ids,
            DateTime timestamp,
            CancellationToken cancellationToken = default
            ) where T : class, IEventSourcedAggregate, new()
        {
            return LoadAll<T>(
                ids.Select(id => (Id: id, Timestamp: timestamp)),
                cancellationToken
                );
        }

        public async Task<IEnumerable<Result<T, Errors>>> LoadAllThatExist<T>(
            IEnumerable<ValueObjects.Id> possibleIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return (await LoadAllRaw<T>(
                  possibleIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
                .Where(((ValueObjects.Id id, T? aggregate) t) =>
                    HasAggregateNeverExisted(t.aggregate) ||
                    DoesAggregateExist(t.aggregate)
                    ) // Exclude the aggregates that existed once but not at the given timestamp, that is, the ones whose version is `0`. But include aggregates that never existed at all, that is, the ones that are `null`, with the effect that they are reported as errors by `BuildResult`.
                .Select(((ValueObjects.Id id, T? aggregate) t) =>
                    BuildResult(t.id, t.aggregate)
                    );
        }

        public async Task<IEnumerable<Result<T, Errors>>> LoadAllThatExisted<T>(
            IEnumerable<Guid> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return (await LoadAllRaw<T>(
                  possibleIds.Select(id => (id, timestamp)),
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
                .Where((((Guid, DateTime) idAndTimestamp, T? aggregate) t) =>
                    HasAggregateNeverExisted(t.aggregate) ||
                    DoesAggregateExist(t.aggregate)
                    ) // Exclude the aggregates that existed once but not at the given timestamp, that is, the ones whose version is `0`. But include aggregates that never existed at all, that is, the ones that are `null`, with the effect that they are reported as errors by `BuildResult`.
                .Select((((Guid id, DateTime timestamp) idAndTimestamp, T? aggregate) t) =>
                    BuildResult(t.idAndTimestamp.id, t.aggregate)
                    );
        }

        public Task<IEnumerable<Result<T, Errors>>> LoadAllThatExisted<T>(
            IEnumerable<ValueObjects.Id> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return LoadAllThatExisted<T>(
                possibleIds.Select(x => (Guid)x),
                timestamp,
                cancellationToken
                );
        }

        private async Task<IEnumerable<(DateTime, IEnumerable<(Guid, T?)>)>> LoadAllBatchedRaw<T>(
            IEnumerable<(DateTime, IEnumerable<Guid>)> timestampsAndIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var batch = _session.CreateBatchQuery();
            var tasks = timestampsAndIds.Select(((DateTime timestamp, IEnumerable<Guid> ids) t) =>
                Task.WhenAll(
                    t.ids.Select(id =>
                        batch.Events.AggregateStream<T>(id, timestamp: t.timestamp.ToUniversalTime())
                        )
                      // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
                      .ToList()
                      )
                ).ToList(); // Force evaluation of the lazily evaluated `Select` before executing the batch.
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var aggregates = await Task.WhenAll(tasks).ConfigureAwait(false);
            return
              timestampsAndIds
              .Zip(aggregates, ((DateTime timestamp, IEnumerable<Guid> ids) t, IEnumerable<T> aggregates) =>
                  (t.timestamp,
                   t.ids.Zip(aggregates, (id, aggregate) => (id, (T?)aggregate))
                   )
                  );
        }

        public async Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllBatched<T>(
            IEnumerable<(DateTime, IEnumerable<Guid>)> timestampsAndIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return (await LoadAllBatchedRaw<T>(
                  timestampsAndIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .Select(((DateTime timestamp, IEnumerable<(Guid, T?)> idsAndAggregates) t) =>
                  t.idsAndAggregates.Select(((Guid id, T? aggregate) u) =>
                    BuildResult(u.id, u.aggregate)
                    )
                  );
        }

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllBatched<T>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return LoadAllBatched<T>(
                timestampsAndIds.Select(((ValueObjects.Timestamp timestamp, IEnumerable<ValueObjects.Id> ids) t) =>
                  ((DateTime)t.timestamp,
                   t.ids.Select(id => (Guid)id))
                  )
                );
        }

        public async Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllThatExistedBatched<T>(
            IEnumerable<(DateTime, IEnumerable<Guid>)> timestampsAndPossibleIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return
              (await LoadAllBatchedRaw<T>(
                  timestampsAndPossibleIds,
                  cancellationToken
                  )
               .ConfigureAwait(false)
                )
              .Select(((DateTime timestamp, IEnumerable<(Guid, T?)> idsAndAggregates) t) =>
                  t.idsAndAggregates
                  .Where(((Guid id, T? aggregate) u) =>
                    HasAggregateNeverExisted(u.aggregate) ||
                    DoesAggregateExist(u.aggregate)
                  ) // Exclude the aggregates that existed once but not at the given timestamp, that is, the ones whose version is `0`. But include aggregates that never existed at all, that is, the ones that are `null`, with the effect that they are reported as errors by `BuildResult`.
                  .Select(((Guid id, T? aggregate) u) =>
                    BuildResult(u.id, u.aggregate))
                  );
        }

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllThatExistedBatched<T>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndPossibleIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return LoadAllThatExistedBatched<T>(
                timestampsAndPossibleIds.Select(((ValueObjects.Timestamp timestamp, IEnumerable<ValueObjects.Id> possibleIds) t) =>
                  ((DateTime)t.timestamp,
                   t.possibleIds.Select(possibleId => (Guid)possibleId))
                  ),
                cancellationToken
                );
        }

        private bool DoesStreamStateExist<T>(
            [NotNullWhen(true)] StreamState? streamState,
            DateTime timestamp
            )
          where T : class, IEventSourcedAggregate, new()
        {
            return
              streamState != null &&
              streamState.AggregateType == typeof(T) &&
              streamState.Created <= timestamp.ToUniversalTime();
        }

        private bool HasAggregateNeverExisted<T>([NotNullWhen(false)] T? aggregate)
          where T : class, IEventSourcedAggregate, new()
        {
            return aggregate is null;
        }

        private bool DoesAggregateExist<T>([NotNullWhen(true)] T? aggregate)
          where T : class, IEventSourcedAggregate, new()
        {
            return
              !(aggregate is null) &&
              aggregate.Version >= 1 &&
              !aggregate.Deleted;
        }

        private Result<T, Errors> BuildResult<T>(Guid id, T? aggregate)
          where T : class, IEventSourcedAggregate
        {
            aggregate?.EnsureValid();
            if (aggregate is null || aggregate.Version == 0 || aggregate.Deleted)
            {
                return Result.Failure<T, Errors>(BuildNonExistentModelError(id));
            }
            return Result.Ok<T, Errors>(aggregate!);
        }

        ////////////
        // Models //
        ////////////

        public async
          Task<IEnumerable<Result<TModel, Errors>>>
          GetModels<TModel, TAggregate, TCreatedEvent>(
            CancellationToken cancellationToken
            )
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TCreatedEvent : ICreatedEvent
        {
            var possibleIds = await QueryModelIds<TCreatedEvent>(cancellationToken).ConfigureAwait(false);
            return
              (await LoadAllThatExist<TAggregate>(possibleIds, cancellationToken).ConfigureAwait(false))
              .Select(result =>
                  result.Bind(a => a.ToModel())
                  );
        }

        public async
          Task<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
          GetModelsAtTimestamps<TModel, TAggregate, TCreatedEvent>(
            IEnumerable<ValueObjects.Timestamp> timestamps,
            CancellationToken cancellationToken
            )
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TCreatedEvent : ICreatedEvent
        {
            var possibleIds = await QueryModelIds<TCreatedEvent>(cancellationToken).ConfigureAwait(false);
            return
              (await LoadAllThatExistedBatched<TAggregate>(
                 timestamps.Select(timestamp => (timestamp, possibleIds)),
                 cancellationToken
                 )
               .ConfigureAwait(false)
                )
              .Select(results =>
                  Result.Ok<IEnumerable<Result<TModel, Errors>>, Errors>(
                    results.Select(result =>
                      result.Bind(a => a.ToModel())
                      )
                    )
                  );
        }

        private async Task<IEnumerable<ValueObjects.Id>> QueryModelIds<TCreatedEvent>(
            CancellationToken cancellationToken
            )
          where TCreatedEvent : ICreatedEvent
        {
            return
              (await QueryEvents<TCreatedEvent>()
              .Select(e => e.AggregateId)
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false)
              )
              .Select(id => (ValueObjects.Id)id);
        }

        //////////////////
        // Associations //
        //////////////////

        public
          Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
          GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            CancellationToken cancellationToken
            )
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociationsOrAssociatesOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate>(
                timestampedModelIds,
                QueryForwardAssociationIds<TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
          GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
              IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
              CancellationToken cancellationToken
              )
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociationsOrAssociatesOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate>(
                timestampedModelIds,
                QueryBackwardAssociationIds<TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public
          Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
          GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            CancellationToken cancellationToken
            )
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociationsOrAssociatesOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate>(
                timestampedModelIds,
                QueryForwardAssociationIds<TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public async
          Task<IEnumerable<Result<TAssociationModel, Errors>>>
          GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            CancellationToken cancellationToken
            )
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return
              (
               await GetBackwardOneToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                 timestampedModelIds,
                 cancellationToken
                 )
               .ConfigureAwait(false)
              )
              .Select(associationsResult =>
                  associationsResult.Bind(associationResults =>
                    associationResults.First() // Using `First` here is safe because `GetBackwardOneToManyAssociationsOfModels` makes sure that the count of `associationResults` is `1`.
                    )
                  );
        }

        protected async
          Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
          GetBackwardOneToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            CancellationToken cancellationToken
            )
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            // TODO Express the post-condition that the count of `IEnumerable<Result<TAssociationModel, Errors>>, Errors` is `1`.
            return
              (
               await GetAssociationsOrAssociatesOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate>(
                 timestampedModelIds,
                 QueryBackwardAssociationIds<TAssociationAddedEvent>,
                 cancellationToken
                 )
               .ConfigureAwait(false)
              )
              .Zip(timestampedModelIds, (associationsResult, timestampedModelId) =>
                  associationsResult.Bind(associationResults =>
                    associationResults.Count() is 1
                    ? Result.Ok<IEnumerable<Result<TAssociationModel, Errors>>, Errors>(associationResults)
                    : Result.Failure<IEnumerable<Result<TAssociationModel, Errors>>, Errors>(
                      Errors.One(
                        message: $"The model of type {typeof(TAssociateModel)} with identifier {timestampedModelId.Id} at timestamp {timestampedModelId.Timestamp} does not have one one-to-many association of type {typeof(TAssociationModel)} but {associationResults.Count()}",
                        code: ErrorCodes.InvalidState
                        )
                      )
                    )
                  );
        }

        private async
          Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
          GetAssociationsOrAssociatesOfModels<TModel, TAssociateModel, TAggregate, TAssociateAggregate>(
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            Func<IEnumerable<ValueObjects.Id>, CancellationToken, Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associateId)>>> queryAssociateIds,
            CancellationToken cancellationToken
            )
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
        {
            var modelIds =
              timestampedModelIds
              .Select(timestampedId => timestampedId.Id);
            var doModelIdsExist =
              await Exist<TAggregate>(
                  timestampedModelIds,
                  cancellationToken
                  )
              .ConfigureAwait(false);
            var existingModelIds =
              modelIds.Zip(doModelIdsExist)
              .Where(x => x.Second)
              .Select(x => x.First);
            // TODO Use LINQs `GroupBy` once it has been implemented for Marten, see https://github.com/JasperFx/marten/issues/569
            var modelIdToAssociateIds =
              (await queryAssociateIds(
                  existingModelIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .ToLookup(
                t => t.modelId,
                t => t.associateId
                );
            var timestampsAndAssociatesIds =
              timestampedModelIds
              .Select(timestampedId => (
                    timestampedId.Timestamp,
                    modelIdToAssociateIds.Contains(timestampedId.Id)
                    ? modelIdToAssociateIds[timestampedId.Id]
                    : Enumerable.Empty<ValueObjects.Id>()
                    )
                  );
            return
              (await LoadAllThatExistedBatched<TAssociateAggregate>(
                  timestampsAndAssociatesIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .Zip(modelIds.Zip(doModelIdsExist), (results, modelIdAndExists) =>
                  modelIdAndExists.Second
                  ? Result.Ok<IEnumerable<Result<TAssociateModel, Errors>>, Errors>(
                    results.Select(result =>
                      result.Bind(a => a.ToModel())
                      )
                    )
                  : Result.Failure<IEnumerable<Result<TAssociateModel, Errors>>, Errors>(
                    Errors.One(
                      message: $"There is no model with id {modelIdAndExists.First}",
                      code: ErrorCodes.NonExistentModel
                      )
                    )
                  );
        }

        private async
          Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associationId)>>
          QueryForwardAssociationIds<TAssociationAddedEvent>(
            IEnumerable<ValueObjects.Id> modelIds,
            CancellationToken cancellationToken
            )
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            var modelGuids = modelIds.Select(modelId => (Guid)modelId).ToArray();
            return (await QueryEvents<TAssociationAddedEvent>()
                .Where(e => e.ParentId.IsOneOf(modelGuids))
                .Select(e => new { ModelId = e.ParentId, AssociationId = e.AggregateId })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false))
              .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociationId));
        }

        private async
          Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associationId)>>
          QueryBackwardAssociationIds<TAssociationAddedEvent>(
            IEnumerable<ValueObjects.Id> modelIds,
            CancellationToken cancellationToken
            )
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            var modelGuids = modelIds.Select(modelId => (Guid)modelId).ToArray();
            return (await QueryEvents<TAssociationAddedEvent>()
                .Where(e => e.AssociateId.IsOneOf(modelGuids))
                .Select(e => new { ModelId = e.AssociateId, AssociationId = e.AggregateId })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false))
              .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociationId));
        }

        ////////////////
        // Associates //
        ////////////////

        public
          Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
          GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where TAssociationModel : Models.IManyToManyAssociation
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                timestampedIds,
                association => association.AssociateId,
                GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public
          Task<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
          GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where TAssociationModel : Models.IManyToManyAssociation
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociatesOfModels<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate>(
                timestampedIds,
                association => association.ParentId,
                GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public
          Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
          GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where TAssociationModel : Models.IOneToManyAssociation
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                timestampedIds,
                association => association.AssociateId,
                GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public async
          Task<IEnumerable<Result<TModel, Errors>>>
          GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where TAssociationModel : Models.IOneToManyAssociation
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return
              (
              await GetAssociatesOfModels<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate>(
                 timestampedIds,
                 association => association.ParentId,
                 GetBackwardOneToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                 cancellationToken
                 )
               .ConfigureAwait(false)
              )
              .Zip(timestampedIds, (associatesResult, timestampedId) =>
                  associatesResult.Bind(associateResults =>
                    associateResults.Count() is 1
                    ? associateResults.First()
                    : Result.Failure<TModel, Errors>(
                      Errors.One(
                        message: $"The model of type {typeof(TAssociateModel)} with identifier {timestampedId.Id} at timestamp {timestampedId.Timestamp} does not have one one-to-many associate of type {typeof(TAssociateModel)} through {typeof(TAssociationModel)} but {associateResults.Count()}",
                        code: ErrorCodes.InvalidState
                        )
                      )
                    )
                  );
        }

        private async
          Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
          GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            Func<TAssociationModel, ValueObjects.Id> getAssociateId,
            Func<IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>> getAssociations,
            CancellationToken cancellationToken
            )
          where TAssociationModel : Models.IAssociation
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
        {
            var results =
              await getAssociations(
                timestampedIds,
                cancellationToken
                )
              .ConfigureAwait(false);
            var timestampsAndAssociatesIds =
              timestampedIds
              .Zip(results, (timestampedId, result) => (
                    timestampedId.Timestamp,
                    result.IsSuccess
                    ? result.Value.Where(r => r.IsSuccess).Select(r => getAssociateId(r.Value))
                    : Enumerable.Empty<ValueObjects.Id>()
                    )
                  );
            var batchedAssociateResults =
              await LoadAllBatched<TAssociateAggregate>(
                  timestampsAndAssociatesIds,
                  cancellationToken
                  )
              .ConfigureAwait(false);
            var associateResultsEnumerators =
              batchedAssociateResults.Select(associateResults =>
                  associateResults.GetEnumerator()
                  );
            // TODO For some reason evaluating the result lazily and not
            // forcing eager execution by calling `ToList` below, results in
            // `Zip` (and `Select`) being executed multiple times and thus in
            // the enumerators being used multiple times, which is not possible
            // because we do not reset them. Figure out why!
            return results.Zip(associateResultsEnumerators, (result, associateResultsEnumerator) =>
                result.Map(associationResults =>
                  associationResults.Select(associationResult =>
                    associationResult.Bind(association =>
                      associateResultsEnumerator.MoveNext()
                      ? associateResultsEnumerator.Current.Bind(associateResult =>
                          associateResult.ToModel()
                          )
                      : Result.Failure<TAssociateModel, Errors>(
                          Errors.One(
                            message: $"There is no more result for the assocation {association}",
                            code: ErrorCodes.NonExistentModel
                            )
                          )
                      )
                    ).ToList().AsEnumerable()
                  )
                ).ToList().AsEnumerable();
        }
    }
}