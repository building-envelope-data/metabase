using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Events;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Aggregates
{
    public interface IAggregateRepositorySession : IAggregateRepositoryReadOnlySession
    {
        public Task<Result<Id, Errors>> Create<T>(
            Func<Guid, ICreatedEvent> newCreatedEvent,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<Id, Errors>> Create<T>(
            ICreatedEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Append<T>(
            TimestampedId timestampedId,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Append<T>(
            TimestampedId timestampedId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Delete<T>(
            Timestamp timestamp,
            IDeletedEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Delete<T>(
            IEnumerable<(Timestamp, IDeletedEvent)> timestampsAndEvents,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Save(
            CancellationToken cancellationToken
            );

        //////////////////
        // Associations //
        //////////////////

        public Task<Result<Id, Errors>> AddManyToManyAssociation<TParent, TAssociation, TAssociate>(
            Func<Guid, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TParent : class, IEventSourcedAggregate, new()
          where TAssociation : class, IManyToManyAssociationAggregate, new()
          where TAssociate : class, IEventSourcedAggregate, new();

        public Task<Result<Id, Errors>> AddOneToManyAssociation<TParent, TAssociation, TAssociate>(
            Func<Guid, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TParent : class, IEventSourcedAggregate, new()
          where TAssociation : class, IOneToManyAssociationAggregate, new()
          where TAssociate : class, IEventSourcedAggregate, new();

        public
          Task<Result<Id, Errors>>
          RemoveManyToManyAssociation<TAssociationAggregate>(
            (Guid from, Guid to) ids,
            Timestamp timestamp,
            Func<Guid, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
          where TAssociationAggregate : class, IManyToManyAssociationAggregate, new();

        public
          Task<Result<Id, Errors>>
          RemoveOneToManyAssociation<TAssociationAggregate>(
            Guid toId,
            Timestamp timestamp,
            Func<Guid, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
          where TAssociationAggregate : class, IOneToManyAssociationAggregate, new();

        public
          Task<Result<bool, Errors>>
          RemoveForwardManyToManyAssociationsOfModel<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            TimestampedId timestampedId,
            Func<Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<Result<bool, Errors>>
          RemoveBackwardManyToManyAssociationsOfModel<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            TimestampedId timestampedId,
            Func<Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<Result<bool, Errors>>
          RemoveForwardOneToManyAssociationsOfModel<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            TimestampedId timestampedId,
            Func<Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent;

        public
          Task<Result<bool, Errors>>
          RemoveBackwardOneToManyAssociationOfModel<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            TimestampedId timestampedId,
            Func<Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent;
    }
}