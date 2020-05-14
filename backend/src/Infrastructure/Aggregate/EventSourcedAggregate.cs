// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using ErrorCodes = Icon.ErrorCodes;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using Icon.Events;
using Newtonsoft.Json;

namespace Icon.Infrastructure.Aggregate
{
    public abstract class EventSourcedAggregate
      : Validatable, IEventSourcedAggregate
    {
        // For indexing our event streams
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        // For protecting the state, i.e. conflict prevention
        public int Version { get; set; }

        protected EventSourcedAggregate()
        {
            Id = Guid.Empty;
            Timestamp = DateTime.MinValue;
            Version = 0;
        }

        protected void ApplyMeta<E>(Marten.Events.Event<E> @event)
          where E : IEvent
        {
            @event.Data.EnsureValid();
            Timestamp = @event.Timestamp.UtcDateTime;
            Version = @event.Version;
        }

        public bool IsVirgin()
        {
            return
               // Id == Guid.Empty && // TODO For some reason the `Id` is set by `AggregateRepositoryReadOnlySession#Load<T>`. Why? How? Who?
               Timestamp == DateTime.MinValue &&
               Version is 0;
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Ok<bool, Errors>(true);

            return
              Result.Combine<bool, Errors>(
                ValidateNonEmpty(Id, nameof(Id)),
                ValidateNotMinValue(Timestamp, nameof(Timestamp)),
                ValidateNonZero(Version, nameof(Version))
                );
        }

        public Result<bool, Errors> ValidateNonVirgin()
        {
            if (IsVirgin())
                return
                  Result.Failure<bool, Errors>(
                      Errors.One(
                        message: $"The aggregate {this} is a virgin",
                        code: ErrorCodes.InvalidState
                        )
                      );

            return Validate();
        }
    }
}