using System;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
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
    public interface IAggregateRepositorySession : IAggregateRepositoryReadOnlySession
    {
        public Task<Result<bool, Errors>> Create<T>(
            Guid id,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Create<T>(
            Guid id,
            IEnumerable<IEvent> events,
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
            ValueObjects.TimestampedId timestampedId,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Delete<T>(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Delete<T>(
            IEnumerable<(ValueObjects.TimestampedId, IEvent)> timestampedIdsAndEvents,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<bool, Errors>> Save(
            CancellationToken cancellationToken
            );
    }
}