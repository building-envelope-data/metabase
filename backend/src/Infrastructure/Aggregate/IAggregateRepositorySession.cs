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
        public Task<Result<ValueObjects.TimestampedId, Errors>> Store<T>(
            Guid id,
            int expectedVersion,
            IEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();

        public Task<Result<ValueObjects.TimestampedId, Errors>> Store<T>(
            Guid id,
            int expectedVersion,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new();
    }
}