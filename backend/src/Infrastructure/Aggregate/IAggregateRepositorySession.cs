using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Icon.Events;
using Marten;
using Marten.Linq;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Infrastructure.Aggregate
{
    public interface IAggregateRepositorySession : IAggregateRepositoryReadOnlySession
    {
        public Task<Result<ValueObjects.Id, Errors>> Create<T>(
            Func<Guid, Events.ICreatedEvent> newCreatedEvent,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<ValueObjects.Id, Errors>> Create<T>(
            ICreatedEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Append<T>(
            ValueObjects.TimestampedId timestampedId,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Append<T>(
            ValueObjects.TimestampedId timestampedId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Delete<T>(
            ValueObjects.Timestamp timestamp,
            IDeletedEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Delete<T>(
            IEnumerable<(ValueObjects.Timestamp, IDeletedEvent)> timestampsAndEvents,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Save(
            CancellationToken cancellationToken
            );

        //////////////////
        // Associations //
        //////////////////

        public Task<Result<ValueObjects.Id, Errors>> AddManyToManyAssociation<TParent, TAssociation, TAssociate>(
            Func<Guid, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TParent : class, IEventSourcedAggregate, new()
          where TAssociation : class, Aggregates.IManyToManyAssociationAggregate, new()
          where TAssociate : class, IEventSourcedAggregate, new();

        public Task<Result<ValueObjects.Id, Errors>> AddOneToManyAssociation<TParent, TAssociation, TAssociate>(
            Func<Guid, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TParent : class, IEventSourcedAggregate, new()
          where TAssociation : class, Aggregates.IOneToManyAssociationAggregate, new()
          where TAssociate : class, IEventSourcedAggregate, new();

        public
          Task<Result<ValueObjects.Id, Errors>>
          RemoveManyToManyAssociation<TAssociationAggregate>(
            (Guid from, Guid to) ids,
            ValueObjects.Timestamp timestamp,
            Func<Guid, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, new();

        public
          Task<Result<ValueObjects.Id, Errors>>
          RemoveOneToManyAssociation<TAssociationAggregate>(
            Guid toId,
            ValueObjects.Timestamp timestamp,
            Func<Guid, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, new();

        public
          Task<Result<bool, Errors>>
          RemoveForwardManyToManyAssociationsOfModel<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<Result<bool, Errors>>
          RemoveBackwardManyToManyAssociationsOfModel<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<Result<bool, Errors>>
          RemoveForwardOneToManyAssociationsOfModel<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<Result<bool, Errors>>
          RemoveBackwardOneToManyAssociationOfModel<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent;
    }
}