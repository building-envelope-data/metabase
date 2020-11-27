using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Marten;
using Marten.Linq;

namespace Infrastructure.Models
{
    public class ModelRepositoryReadOnlySession
        : IDisposable
    {
        private readonly Marten.IDocumentSession _session;
        private bool _disposed;

        // TODO We would like to use `IQuerySession` here, which however does
        // not provide access to an `IEventStore` via a getter `Events`.
        public ModelRepositoryReadOnlySession(
            Marten.IDocumentSession session
            )
        {
            _session = session;
            _disposed = false;
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("This session has been disposed");
            }
        }

        // Inspired by https://github.com/JasperFx/marten/blob/master/src/Marten/QuerySession.cs
        // The recommendation documentation of how to implement the IDisposable interface is slightly different
        // https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.8#idisposable-and-the-inheritance-hierarchy
        ~ModelRepositoryReadOnlySession()
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

        private Errors BuildNonExistentModelError(ValueObjects.Id id)
        {
            return
              Errors.One(
                  message: $"There is no model with id {id}",
                  code: ErrorCodes.NonExistentModel
                  );
        }

        public IMartenQueryable<TEvent> QueryEvents<TEvent>()
          where TEvent : Events.IEvent
        {
            ThrowIfDisposed();
            return _session.Events.QueryRawEventDataOnly<TEvent>();
        }

        public IMartenQueryable<TAggregate> QueryAggregates<TAggregate>()
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            return _session.Query<TAggregate>();
        }

        public async Task<ValueObjects.Id> GenerateNewId(
            CancellationToken cancellationToken
            )
        {
            ThrowIfDisposed();
            var id = ValueObjects.Id.New();
            while (await Exists(id, cancellationToken).ConfigureAwait(false))
            {
                id = ValueObjects.Id.New();
            }
            return id;
        }

        public async Task<bool> Exists(
            ValueObjects.Id id,
            CancellationToken cancellationToken
            )
        {
            ThrowIfDisposed();
            var streamState =
              await _session.Events.FetchStreamStateAsync(
                  id,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            return streamState != null;
        }

        public async Task<bool> Exists<TAggregate>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            var streamState =
              await _session.Events.FetchStreamStateAsync(
                  timestampedId.Id,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            return DoesStreamStateExist<TAggregate>(streamState, timestampedId.Timestamp);
        }

        public async Task<IEnumerable<bool>> Exist<TAggregate>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            var batch = _session.CreateBatchQuery();
            var streamStateTasks =
              timestampedIds.Select(timestampedId =>
                batch.Events.FetchStreamState(timestampedId.Id)
                )
              .ToList(); // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var streamStates = await Task.WhenAll(streamStateTasks).ConfigureAwait(false);
            return timestampedIds.Zip(streamStates, (timestampedId, streamState) =>
                DoesStreamStateExist<TAggregate>(streamState, timestampedId.Timestamp)
                );
        }

        private bool DoesStreamStateExist<TAggregate>(
            [NotNullWhen(true)] Marten.Events.StreamState? streamState,
            ValueObjects.Timestamp timestamp
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            return
              streamState != null &&
              streamState.AggregateType == typeof(TAggregate) &&
              streamState.Created <= timestamp;
        }

        public async Task<Result<int, Errors>> FetchVersion<TAggregate>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken
            ) where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            // TODO For performance reasons it would be great if we could use
            // var expectedVersion = (await _session.Events.FetchStreamStateAsync(id, timestamp: timestamp, token: cancellationToken)).Version;
            // (the parameter `timestamp` is not implemented though)
            // Ask on https://github.com/JasperFx/marten/issues for the parameter `timestamp` to be implemented
            var aggregate =
              await _session.Events.AggregateStreamAsync<TAggregate>(
                  timestampedId.Id,
                  timestamp: timestampedId.Timestamp,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            if (aggregate is null)
            {
                return Result.Failure<int, Errors>(
                    BuildNonExistentModelError(timestampedId.Id)
                    );
            }
            return Result.Success<int, Errors>(aggregate.Version);
        }

        public async Task<Result<ValueObjects.Timestamp, Errors>> FetchTimestamp<TAggregate>(
            ValueObjects.Id id,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            return (await FetchStreamState<TAggregate>(id, cancellationToken).ConfigureAwait(false))
              .Bind(streamState =>
                      ValueObjects.Timestamp.From(streamState.LastTimestamp)
                  );
        }

        public async Task<Result<Type, Errors>> FetchAggregateType(
            ValueObjects.Id id,
            CancellationToken cancellationToken
            )
        {
            return (await FetchStreamState(id, cancellationToken).ConfigureAwait(false))
              .Map(streamState => streamState.AggregateType);
        }

        public async Task<IEnumerable<Result<Type, Errors>>> FetchAggregateTypes(
            IEnumerable<ValueObjects.Id> ids,
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

        public async Task<Result<ValueObjects.TimestampedId, Errors>> TimestampId<TAggregate>(
            ValueObjects.Id id,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            var timestampResult = await FetchTimestamp<TAggregate>(id, cancellationToken).ConfigureAwait(false);
            return ValueObjects.Id.From(id)
              .Bind(nonEmptyId =>
                  timestampResult.Bind(timestamp =>
                    ValueObjects.TimestampedId.From(
                      nonEmptyId, timestamp
                      )
                    )
                  );
        }

        private async
          Task<Result<Marten.Events.StreamState, Errors>>
          FetchStreamState(
            ValueObjects.Id id,
            CancellationToken cancellationToken
            )
        {
            ThrowIfDisposed();
            var streamState =
              await _session.Events.FetchStreamStateAsync(
                  id,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            if (streamState is null)
            {
                return Result.Failure<Marten.Events.StreamState, Errors>(BuildNonExistentModelError(id));
            }
            return Result.Success<Marten.Events.StreamState, Errors>(streamState);
        }

        private async
          Task<Result<Marten.Events.StreamState, Errors>>
          FetchStreamState<TAggregate>(
            ValueObjects.Id id,
            CancellationToken cancellationToken
            )
        {
            ThrowIfDisposed();
            return (await FetchStreamState(id, cancellationToken).ConfigureAwait(false))
              .Bind(streamState =>
                CheckStreamStateType<TAggregate>(id, streamState)
                );
        }

        private async
          Task<IEnumerable<Result<Marten.Events.StreamState, Errors>>>
          FetchStreamStates(
            IEnumerable<ValueObjects.Id> ids,
            CancellationToken cancellationToken
            )
        {
            ThrowIfDisposed();
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
                    ? Result.Failure<Marten.Events.StreamState, Errors>(BuildNonExistentModelError(id))
                    : Result.Success<Marten.Events.StreamState, Errors>(streamState)
                  );
        }

        private async
          Task<IEnumerable<Result<Marten.Events.StreamState, Errors>>>
          FetchStreamStates<TAggregate>(
            IEnumerable<ValueObjects.Id> ids,
            CancellationToken cancellationToken
            )
        {
            ThrowIfDisposed();
            return ids.Zip(await FetchStreamStates(ids, cancellationToken).ConfigureAwait(false), (id, streamStateResult) =>
                  streamStateResult.Bind(streamState =>
                      CheckStreamStateType<TAggregate>(id, streamState)
                      )
                  );
        }

        private
         Result<Marten.Events.StreamState, Errors>
          CheckStreamStateType<TAggregate>(
              ValueObjects.Id id,
              Marten.Events.StreamState streamState
              )
        {
            return streamState.AggregateType == typeof(TAggregate)
            ? Result.Success<Marten.Events.StreamState, Errors>(streamState)
            : Result.Failure<Marten.Events.StreamState, Errors>(
                    Errors.One(
                      message: $"The aggregate with id {id} is of type {streamState.AggregateType} and not of the expected type {typeof(TAggregate)}",
                      code: ErrorCodes.InvalidType
                      )
                    );
        }

        public async Task<Result<TModel, Errors>> Load<TModel, TAggregate>(
            ValueObjects.Id id,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            // Loading the materialized aggregate as follows
            // var aggregate = await _session.LoadAsync<TAggregate>(id, cancellationToken);
            // is not possible because event meta data is not available during
            // inline projection as said on
            // https://martendb.io/documentation/events/projections/
            // in the section on inline projections.
            // Therefore, version and timestamp of snapshots are not set.
            var aggregate =
              await _session.Events.AggregateStreamAsync<TAggregate>(
                  id,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            return BuildResult<TModel, TAggregate>(id, aggregate);
        }

        public async
          Task<(Result<TModel1, Errors>, Result<TModel2, Errors>)>
          Load<TModel1, TAggregate1, TModel2, TAggregate2>(
            (Result<ValueObjects.Id, Errors>, Result<ValueObjects.Id, Errors>) ids,
            CancellationToken cancellationToken
            )
          where TModel1 : Models.IModel
          where TAggregate1 : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel1>, new()
          where TModel2 : Models.IModel
          where TAggregate2 : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel2>, new()
        {
            ThrowIfDisposed();
            return
               (ids.Item1.IsSuccess, ids.Item2.IsSuccess) switch
               {
                   (true, true) =>
                    await Load<TModel1, TAggregate1, TModel2, TAggregate2>(
                      (ids.Item1.Value, ids.Item2.Value),
                      cancellationToken
                      )
                      .ConfigureAwait(false),
                   (true, false) =>
                    (
                      await Load<TModel1, TAggregate1>(ids.Item1.Value, cancellationToken).ConfigureAwait(false),
                      Result.Failure<TModel2, Errors>(ids.Item2.Error)
                    ),
                   (false, true) =>
                    (
                      Result.Failure<TModel1, Errors>(ids.Item1.Error),
                      await Load<TModel2, TAggregate2>(ids.Item2.Value, cancellationToken).ConfigureAwait(false)
                      ),
                   (false, false) =>
                   (
                      Result.Failure<TModel1, Errors>(ids.Item1.Error),
                      Result.Failure<TModel2, Errors>(ids.Item2.Error)
                   )
               };
        }

        public async
          Task<(Result<TModel1, Errors>, Result<TModel2, Errors>)>
          Load<TModel1, TAggregate1, TModel2, TAggregate2>(
            (ValueObjects.Id, ValueObjects.Id) ids,
            CancellationToken cancellationToken
            )
          where TModel1 : Models.IModel
          where TAggregate1 : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel1>, new()
          where TModel2 : Models.IModel
          where TAggregate2 : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel2>, new()
        {
            ThrowIfDisposed();
            var batch = _session.CreateBatchQuery();
            // Loading the materialized aggregates as follows
            // batch.Load<TAggregate>(id);
            // is not possible because event meta data is not available during
            // inline projection as said on
            // https://martendb.io/documentation/events/projections/
            // in the section on inline projections.
            // Therefore, version and timestamp of snapshots are not set.
            var aggregateStreamTasks = (
              batch.Events.AggregateStream<TAggregate1>(ids.Item1),
              batch.Events.AggregateStream<TAggregate2>(ids.Item2)
              );
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            await Task.WhenAll(
                aggregateStreamTasks.Item1,
                aggregateStreamTasks.Item2
                )
              .ConfigureAwait(false);
            return (
                BuildResult<TModel1, TAggregate1>(ids.Item1, aggregateStreamTasks.Item1.Result),
                BuildResult<TModel2, TAggregate2>(ids.Item2, aggregateStreamTasks.Item2.Result)
                );
        }

        public async Task<Result<TModel, Errors>> Load<TModel, TAggregate>(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            var aggregate =
              await _session.Events.AggregateStreamAsync<TAggregate>(
                  id,
                  timestamp: timestamp,
                  token: cancellationToken
                  )
              .ConfigureAwait(false);
            return BuildResult<TModel, TAggregate>(id, aggregate);
        }

        public async Task<Result<TModel, Errors>> LoadX<TModel, TAggregate>(
            ValueObjects.TimestampedId timestampedId,
            Func<ModelRepositoryReadOnlySession, Type, ValueObjects.TimestampedId, CancellationToken, Task<Result<TModel, Errors>>> load,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            var aggregateTypeResult = await FetchAggregateType(timestampedId.Id, cancellationToken).ConfigureAwait(false);
            return await aggregateTypeResult.Bind(async aggregateType =>
                await load(this, aggregateType, timestampedId, cancellationToken).ConfigureAwait(false)
                )
              .ConfigureAwait(false);
        }

        public Task<Result<TModel, Errors>> Load<TModel, TAggregate>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            return Load<TModel, TAggregate>(
                timestampedId.Id,
                timestampedId.Timestamp,
                cancellationToken
                );
        }

        public async Task<IEnumerable<Result<TModel, Errors>>> LoadAll<TModel, TAggregate>(
            IEnumerable<ValueObjects.Id> ids,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            var batch = _session.CreateBatchQuery();
            // Loading the materialized aggregates as follows
            // batch.Load<TAggregate>(id);
            // is not possible because event meta data is not available during
            // inline projection as said on
            // https://martendb.io/documentation/events/projections/
            // in the section on inline projections.
            // Therefore, version and timestamp of snapshots are not set.
            var aggregateStreamTasks =
              ids.Select(id => batch.Events.AggregateStream<TAggregate>(id))
              // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
              .ToList();
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var aggregates = await Task.WhenAll(aggregateStreamTasks).ConfigureAwait(false);
            return ids.Zip(aggregates, BuildResult<TModel, TAggregate>);
        }

        /* public async Task<IEnumerable<TAggregate>> LoadAll<TModel, TAggregate>(ValueObjects.Timestamp timestamp, CancellationToken cancellationToken) where TAggregate : class, Aggregates.IEventSourcedAggregate, new() */
        /* { */
        /*         var aggregateIds = await _session.Query<TAggregate>() */
        /*           .Select(a => a.Id) */
        /*           .ToListAsync(cancellationToken); */
        /*         return await LoadAll<TModel, TAggregate>(aggregateIds, timestamp, cancellationToken); */
        /* } */

        private async Task<IEnumerable<(ValueObjects.Id, TAggregate?)>> LoadAllRaw<TAggregate>(
            IEnumerable<ValueObjects.Id> ids,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            var batch = _session.CreateBatchQuery();
            var aggregateStreamTasks =
              ids.Select(id => batch.Events.AggregateStream<TAggregate>(id))
              // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
              .ToList();
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var aggregates = await Task.WhenAll(aggregateStreamTasks).ConfigureAwait(false);
            return ids.Zip(aggregates,
                (id, aggregate) => (id, (TAggregate?)aggregate)
                );
        }

        private async Task<IEnumerable<(ValueObjects.TimestampedId, TAggregate?)>> LoadAllRaw<TAggregate>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            var batch = _session.CreateBatchQuery();
            var aggregateStreamTasks =
              timestampedIds
              .Select(timestampedId // There sadly is no proper tuple deconstruction in lambdas yet. For details see https://github.com/dotnet/csharplang/issues/258
                  => batch.Events.AggregateStream<TAggregate>(
                    timestampedId.Id,
                    timestamp: timestampedId.Timestamp
                    )
                  )
              // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
              .ToList();
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var aggregates = await Task.WhenAll(aggregateStreamTasks).ConfigureAwait(false);
            return timestampedIds.Zip(aggregates,
                (timestampedId, aggregate) => (timestampedId, (TAggregate?)aggregate)
                );
        }

        public async Task<IEnumerable<Result<TModel, Errors>>> LoadAll<TModel, TAggregate>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            return (await LoadAllRaw<TAggregate>(
                  timestampedIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .Select(((ValueObjects.TimestampedId timestampedId, TAggregate? aggregate) t) =>
                  BuildResult<TModel, TAggregate>(t.timestampedId.Id, t.aggregate)
                  );
        }

        public async Task<IEnumerable<Result<TModel, Errors>>> LoadAllX<TModel>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            Func<ModelRepositoryReadOnlySession, Type, IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<TModel, Errors>>>> loadAll,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
        {
            ThrowIfDisposed();
            var aggregateTypeResults = await FetchAggregateTypes(
                timestampedIds.Select(timestampedId => timestampedId.Id),
                cancellationToken
                )
              .ConfigureAwait(false);
            var aggregateTypeToIdsAndTimestamps =
              timestampedIds.Zip(aggregateTypeResults)
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
                          return Result.Failure<TModel, Errors>(
                                  Errors.One(
                                    message: $"There is no more result of aggregate type {aggregateType}",
                                    code: ErrorCodes.NonExistentModel
                                    )
                                  );
                      }
                        )
                );
        }

        // TODO Find a better name and place!
        private async
         Task<IEnumerable<Result<TValue, Errors>>>
         Do<TArg, TValue>(
          IEnumerable<Result<TArg, Errors>> argResults,
          Func<IEnumerable<TArg>, CancellationToken, Task<IEnumerable<Result<TValue, Errors>>>> what,
          CancellationToken cancellationToken
        )
        {
            ThrowIfDisposed();
            var successfulArgs =
              argResults
              .Where(result => result.IsSuccess)
              .Select(result => result.Value);
            var valueResultsEnumerator =
              (await what(successfulArgs, cancellationToken).ConfigureAwait(false))
              .GetEnumerator();
            return argResults.Select(argResult =>
                    argResult.Bind(arg =>
                          valueResultsEnumerator.MoveNext()
                          ? valueResultsEnumerator.Current
                          : Result.Failure<TValue, Errors>(
                                  Errors.One(
                                    message: $"There is no more result for argument {arg}",
                                    code: ErrorCodes.InvalidState
                                    )
                                  )
                        )
                );
        }

        public Task<IEnumerable<Result<TModel, Errors>>> LoadAll<TModel, TAggregate>(
            IEnumerable<ValueObjects.Id> ids,
            ValueObjects.Timestamp timestamp,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            return Do(
                ids.Select(id => ValueObjects.TimestampedId.From(id, timestamp)),
                LoadAll<TModel, TAggregate>,
                cancellationToken
            );
        }

        public async Task<IEnumerable<Result<TModel, Errors>>> LoadAllThatExist<TModel, TAggregate>(
            IEnumerable<ValueObjects.Id> possibleIds,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            return (await LoadAllRaw<TAggregate>(
                  possibleIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
                .Where(((ValueObjects.Id id, TAggregate? aggregate) t) =>
                    HasAggregateNeverExisted(t.aggregate) ||
                    DoesAggregateExist(t.aggregate)
                    ) // Exclude the aggregates that existed once but not at the given timestamp, that is, the ones whose version is `0`. But include aggregates that never existed at all, that is, the ones that are `null`, with the effect that they are reported as errors by `BuildResult<TModel, TAggregate>`.
                .Select(((ValueObjects.Id id, TAggregate? aggregate) t) =>
                    BuildResult<TModel, TAggregate>(t.id, t.aggregate)
                    );
        }

        public async Task<IEnumerable<Result<TModel, Errors>>> LoadAllThatExisted<TModel, TAggregate>(
            IEnumerable<ValueObjects.Id> possibleIds,
            ValueObjects.Timestamp timestamp,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            return await Do<ValueObjects.TimestampedId, TModel>(
              possibleIds.Select(id => ValueObjects.TimestampedId.From(id, timestamp)),
              async (possibleTimestampedIds, cancellationToken) =>
                (await LoadAllRaw<TAggregate>(possibleTimestampedIds, cancellationToken).ConfigureAwait(false))
                .Where(((ValueObjects.TimestampedId timestampedId, TAggregate? aggregate) t) =>
                    HasAggregateNeverExisted(t.aggregate) ||
                    DoesAggregateExist(t.aggregate)
                    ) // Exclude the aggregates that existed once but not at the given timestamp, that is, the ones whose version is `0`. But include aggregates that never existed at all, that is, the ones that are `null`, with the effect that they are reported as errors by `BuildResult<TModel, TAggregate>`.
                .Select(((ValueObjects.TimestampedId timestampedId, TAggregate? aggregate) t) =>
                    BuildResult<TModel, TAggregate>(t.timestampedId.Id, t.aggregate)
                    ),
              cancellationToken
              )
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Result<TModel, Errors>>> LoadAllThatExisted<TModel, TAggregate>(
            IEnumerable<ValueObjects.TimestampedId> possibleTimestampedIds,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            return (
                await LoadAllRaw<TAggregate>(
                  possibleTimestampedIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
                .Where(((ValueObjects.TimestampedId timestampedId, TAggregate? aggregate) t) =>
                    HasAggregateNeverExisted(t.aggregate) ||
                    DoesAggregateExist(t.aggregate)
                    ) // Exclude the aggregates that existed once but not at the given timestamp, that is, the ones whose version is `0`. But include aggregates that never existed at all, that is, the ones that are `null`, with the effect that they are reported as errors by `BuildResult<TModel, TAggregate>`.
                .Select(((ValueObjects.TimestampedId timestampedId, TAggregate? aggregate) t) =>
                    BuildResult<TModel, TAggregate>(t.timestampedId.Id, t.aggregate)
                    );
        }

        private async Task<IEnumerable<(ValueObjects.Timestamp, IEnumerable<(ValueObjects.Id, TAggregate?)>)>> LoadAllBatchedRaw<TAggregate>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndIds,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            var batch = _session.CreateBatchQuery();
            var tasks = timestampsAndIds.Select(((ValueObjects.Timestamp timestamp, IEnumerable<ValueObjects.Id> ids) t) =>
                Task.WhenAll(
                    t.ids.Select(id =>
                        batch.Events.AggregateStream<TAggregate>(id, timestamp: t.timestamp)
                        )
                      // Turning the `System.Linq.Enumerable+SelectListIterator` into a list forces the lazily evaluated `Select` to be evaluated whereby the queries are added to the batch query.
                      .ToList()
                      )
                ).ToList(); // Force evaluation of the lazily evaluated `Select` before executing the batch.
            await batch.Execute(cancellationToken).ConfigureAwait(false);
            var aggregates = await Task.WhenAll(tasks).ConfigureAwait(false);
            return
              timestampsAndIds
              .Zip(aggregates, ((ValueObjects.Timestamp timestamp, IEnumerable<ValueObjects.Id> ids) t, IEnumerable<TAggregate> aggregates) =>
                  (t.timestamp,
                   t.ids.Zip(aggregates, (id, aggregate) => (id, (TAggregate?)aggregate))
                   )
                  );
        }

        public async Task<IEnumerable<IEnumerable<Result<TModel, Errors>>>> LoadAllBatched<TModel, TAggregate>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndIds,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            return (await LoadAllBatchedRaw<TAggregate>(
                  timestampsAndIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .Select(((ValueObjects.Timestamp timestamp, IEnumerable<(ValueObjects.Id, TAggregate?)> idsAndAggregates) t) =>
                  t.idsAndAggregates.Select(((ValueObjects.Id id, TAggregate? aggregate) u) =>
                    BuildResult<TModel, TAggregate>(u.id, u.aggregate)
                    )
                  );
        }

        public async Task<IEnumerable<IEnumerable<Result<TModel, Errors>>>> LoadAllThatExistedBatched<TModel, TAggregate>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndPossibleIds,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            ThrowIfDisposed();
            return
              (await LoadAllBatchedRaw<TAggregate>(
                  timestampsAndPossibleIds,
                  cancellationToken
                  )
               .ConfigureAwait(false)
                )
              .Select(((ValueObjects.Timestamp timestamp, IEnumerable<(ValueObjects.Id, TAggregate?)> idsAndAggregates) t) =>
                  t.idsAndAggregates
                  .Where(((ValueObjects.Id id, TAggregate? aggregate) u) =>
                    HasAggregateNeverExisted(u.aggregate) ||
                    DoesAggregateExist(u.aggregate)
                  ) // Exclude the aggregates that existed once but not at the given timestamp, that is, the ones whose version is `0`. But include aggregates that never existed at all, that is, the ones that are `null`, with the effect that they are reported as errors by `BuildResult<TModel, TAggregate>`.
                  .Select(((ValueObjects.Id id, TAggregate? aggregate) u) =>
                    BuildResult<TModel, TAggregate>(u.id, u.aggregate))
                  );
        }

        private bool HasAggregateNeverExisted<TAggregate>(
            [NotNullWhen(false)] TAggregate? aggregate
            )
          where TAggregate : class
        {
            return aggregate is null;
        }

        private bool DoesAggregateExist<TAggregate>(
            [NotNullWhen(true)] TAggregate? aggregate
            )
          where TAggregate : class, Aggregates.IAggregate
        {
            return
              !(aggregate is null) &&
              aggregate.Version >= 1 &&
              !aggregate.Deleted;
        }

        private Result<TModel, Errors> BuildResult<TModel, TAggregate>(
            ValueObjects.Id id,
            TAggregate? aggregate
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
        {
            if (aggregate is null ||
                aggregate.Version == 0 ||
                aggregate.Deleted
                )
            {
                return Result.Failure<TModel, Errors>(
                    BuildNonExistentModelError(id)
                    );
            }
            return aggregate!.ToModel()
              .Bind(model =>
                  (Guid)model.Id == aggregate.Id && (DateTime)model.Timestamp == aggregate.Timestamp
                  ? Result.Success<TModel, Errors>(model)
                  : Result.Failure<TModel, Errors>(
                      Errors.One(
                        message: $"The conversion of the aggregate {aggregate} to the model {model} did not preserve the aggregate's identifier {aggregate.Id} or timestamp {aggregate.Timestamp} which became {(Guid)model.Id} and {(DateTime)model.Timestamp}",
                        code: ErrorCodes.InvalidValue
                        )
                    )
                  );
        }

        ////////////
        // Models //
        ////////////

        public async
          Task<IEnumerable<Result<TModel, Errors>>>
          GetModels<TModel, TAggregate, TCreatedEvent>(
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TCreatedEvent : Events.ICreatedEvent
        {
            var possibleIds = await QueryModelIds<TCreatedEvent>(cancellationToken).ConfigureAwait(false);
            return await LoadAllThatExist<TModel, TAggregate>(possibleIds, cancellationToken).ConfigureAwait(false);
        }

        public async
          Task<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
          GetModelsAtTimestamps<TModel, TAggregate, TCreatedEvent>(
            IEnumerable<ValueObjects.Timestamp> timestamps,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TCreatedEvent : Events.ICreatedEvent
        {
            var possibleIds = await QueryModelIds<TCreatedEvent>(cancellationToken).ConfigureAwait(false);
            return
              (await LoadAllThatExistedBatched<TModel, TAggregate>(
                 timestamps.Select(timestamp => (timestamp, possibleIds)),
                 cancellationToken
                 )
               .ConfigureAwait(false)
                )
              .Select(results =>
                  Result.Success<IEnumerable<Result<TModel, Errors>>, Errors>(results)
                  );
        }

        private async Task<IEnumerable<ValueObjects.Id>> QueryModelIds<TCreatedEvent>(
            CancellationToken cancellationToken
            )
          where TCreatedEvent : Events.ICreatedEvent
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
          where TModel : Models.IModel
          where TAssociationModel : Models.IManyToManyAssociation
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
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
          where TAssociateModel : Models.IModel
          where TAssociationModel : Models.IManyToManyAssociation
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
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
          where TModel : Models.IModel
          where TAssociationModel : Models.IOneToManyAssociation
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
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
          where TAssociateModel : Models.IModel
          where TAssociationModel : Models.IOneToManyAssociation
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
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
          where TAssociateModel : Models.IModel
          where TAssociationModel : Models.IOneToManyAssociation
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
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
                    ? Result.Success<IEnumerable<Result<TAssociationModel, Errors>>, Errors>(associationResults)
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
          where TModel : Models.IModel
          where TAssociateModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
        {
            // TODO? Make sure that model is valid by loading it?
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
              (await LoadAllThatExistedBatched<TAssociateModel, TAssociateAggregate>(
                  timestampsAndAssociatesIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .Zip(modelIds.Zip(doModelIdsExist), (results, modelIdAndExists) =>
                  modelIdAndExists.Second
                  ? Result.Success<IEnumerable<Result<TAssociateModel, Errors>>, Errors>(results)
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
          where TModel : Models.IModel
          where TAssociationModel : Models.IManyToManyAssociation
          where TAssociateModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociatesOfModels<TAssociationModel, TAssociateModel, TAssociationAggregate, TAssociateAggregate>(
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
          where TAssociateModel : Models.IModel
          where TAssociationModel : Models.IManyToManyAssociation
          where TModel : Models.IModel
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociatesOfModels<TAssociationModel, TModel, TAssociationAggregate, TAggregate>(
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
          where TModel : Models.IModel
          where TAssociationModel : Models.IOneToManyAssociation
          where TAssociateModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return GetAssociatesOfModels<TAssociationModel, TAssociateModel, TAssociationAggregate, TAssociateAggregate>(
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
          where TAssociateModel : Models.IModel
          where TAssociationModel : Models.IOneToManyAssociation
          where TModel : Models.IModel
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return
              (
              await GetAssociatesOfModels<TAssociationModel, TModel, TAssociationAggregate, TAggregate>(
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
          GetAssociatesOfModels<TAssociationModel, TAssociateModel, TAssociationAggregate, TAssociateAggregate>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            Func<TAssociationModel, ValueObjects.Id> getAssociateId,
            Func<IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>> getAssociations,
            CancellationToken cancellationToken
            )
          where TAssociationModel : IAssociation
          where TAssociateModel : Models.IModel
          where TAssociationAggregate : class, Aggregates.IAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
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
              await LoadAllBatched<TAssociateModel, TAssociateAggregate>(
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
                      ? associateResultsEnumerator.Current
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