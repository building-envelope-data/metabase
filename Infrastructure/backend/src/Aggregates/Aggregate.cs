// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using CSharpFunctionalExtensions;
using Infrastructure.Events;
using Infrastructure.ValueObjects;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;
using Validatable = Infrastructure.Validatable;

namespace Infrastructure.Aggregates
{
    public abstract class Aggregate
      : Validatable, IAggregate
    {
        // For indexing our event streams
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        // For protecting the state, i.e. conflict prevention
        public int Version { get; set; }

        public bool Deleted { get; set; }

        protected Aggregate()
        {
            Id = Guid.Empty;
            Timestamp = DateTime.MinValue;
            Version = 0;
            Deleted = false;
        }

        protected void ApplyMeta<E>(Marten.Events.Event<E> @event)
          where E : IEvent
        {
            @event.Data.EnsureValid();
            Timestamp = @event.Timestamp.UtcDateTime;
            Version = @event.Version;
        }

        protected void ApplyDeleted<E>(Marten.Events.Event<E> @event)
          where E : IDeletedEvent
        {
            ApplyMeta(@event);
            Delete();
        }

        protected void Delete()
        {
            Deleted = true;
        }

        public bool IsVirgin()
        {
            return
               // Id == Guid.Empty && // TODO For some reason the `Id` is set by `ModelRepositoryReadOnlySession#Load<T>`. Why? How? Who?
               Timestamp == DateTime.MinValue &&
               Version is 0 &&
               !Deleted;
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
            {
                return Result.Ok<bool, Errors>(true);
            }
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
            {
                return
                  Result.Failure<bool, Errors>(
                      Errors.One(
                        message: $"The aggregate {this} is a virgin",
                        code: ErrorCodes.InvalidState
                        )
                      );
            }
            return Validate();
        }
    }
}