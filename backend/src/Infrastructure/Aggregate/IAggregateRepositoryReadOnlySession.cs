using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Marten.Linq;
using CancellationToken = System.Threading.CancellationToken;

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
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<(Result<T1, Errors>, Result<T2, Errors>)> Load<T1, T2>(
            (Guid, Guid) ids,
            CancellationToken cancellationToken = default
            )
          where T1 : class, IEventSourcedAggregate, new()
          where T2 : class, IEventSourcedAggregate, new();

        public Task<Result<T, Errors>> Load<T>(
            Guid id,
            DateTime timestamp,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<T, Errors>> LoadX<T>(
            Guid id,
            DateTime timestamp,
            Func<IAggregateRepositoryReadOnlySession, Type, Guid, DateTime, CancellationToken, Task<Result<T, Errors>>> load,
            CancellationToken cancellationToken = default
            );

        public Task<Result<T, Errors>> Load<T>(
            ValueObjects.TimestampedId timestampedId,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<(Guid, DateTime)> idsAndTimestamps,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAllX<T>(
            IEnumerable<(Guid, DateTime)> idsAndTimestamps,
            Func<IAggregateRepositoryReadOnlySession, Type, IEnumerable<(Guid, DateTime)>, CancellationToken, Task<IEnumerable<Result<T, Errors>>>> loadAll,
            CancellationToken cancellationToken = default
            );

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<Guid> ids,
            DateTime timestamp,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAll<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAllX<T>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            Func<IAggregateRepositoryReadOnlySession, Type, IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<T, Errors>>>> loadAll,
            CancellationToken cancellationToken = default
            );

        public Task<IEnumerable<Result<T, Errors>>> LoadAllThatExist<T>(
            IEnumerable<ValueObjects.Id> possibleIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAllThatExisted<T>(
            IEnumerable<Guid> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<Result<T, Errors>>> LoadAllThatExisted<T>(
            IEnumerable<ValueObjects.Id> possibleIds,
            DateTime timestamp,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllBatched<T>(
            IEnumerable<(DateTime, IEnumerable<Guid>)> timestampsAndIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllBatched<T>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllThatExistedBatched<T>(
            IEnumerable<(DateTime, IEnumerable<Guid>)> timestampsAndPossibleIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<IEnumerable<IEnumerable<Result<T, Errors>>>> LoadAllThatExistedBatched<T>(
            IEnumerable<(ValueObjects.Timestamp, IEnumerable<ValueObjects.Id>)> timestampsAndPossibleIds,
            CancellationToken cancellationToken = default
            )
          where T : class, IEventSourcedAggregate, new();

        ////////////
        // Models //
        ////////////

        public
          Task<IEnumerable<Result<TModel, Errors>>>
          GetModels<TModel, TAggregate, TCreatedEvent>(
            CancellationToken cancellationToken
            )
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TCreatedEvent : ICreatedEvent;

        public
          Task<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
          GetModelsAtTimestamps<TModel, TAggregate, TCreatedEvent>(
            IEnumerable<ValueObjects.Timestamp> timestamps,
            CancellationToken cancellationToken
            )
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TCreatedEvent : ICreatedEvent;

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
          where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
          GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
              IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
              CancellationToken cancellationToken
              )
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
          GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            CancellationToken cancellationToken
            )
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<IEnumerable<Result<TAssociationModel, Errors>>>
          GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            CancellationToken cancellationToken
            )
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent;

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
          where TAssociationAddedEvent : Events.IAssociationAddedEvent;

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
          where TAssociationAddedEvent : Events.IAssociationAddedEvent;

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
          where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<IEnumerable<Result<TModel, Errors>>>
          GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
          where TAssociationModel : Models.IOneToManyAssociation
          where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
          where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
          where TAssociationAddedEvent : Events.IAssociationAddedEvent;
    }
}