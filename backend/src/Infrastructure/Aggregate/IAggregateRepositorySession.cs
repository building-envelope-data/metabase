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

        public Task<Result<ValueObjects.Id, Errors>> AddAssociation<TParent, TAssociation, TAssociate>(
            Func<Guid, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            CancellationToken cancellationToken
            )
          where TParent : class, IEventSourcedAggregate, new()
          where TAssociation : class, Aggregates.IManyToManyAssociationAggregate, new()
          where TAssociate : class, IEventSourcedAggregate, new();

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
    }
}