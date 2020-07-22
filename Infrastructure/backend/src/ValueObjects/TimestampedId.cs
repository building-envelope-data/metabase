using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class TimestampedId
      : ValueObject
    {
        public Id Id { get; }
        public Timestamp Timestamp { get; }

        private TimestampedId(
            Id id,
            Timestamp timestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
        }

        public static Result<TimestampedId, Errors> From(
            Guid id,
            DateTime timestamp,
            IReadOnlyList<object>? path = null
            )
        {
            var idResult = ValueObjects.Id.From(id, path: path);
            var timestampResult = ValueObjects.Timestamp.From(timestamp, path: path);

            return Errors.Combine(
                idResult,
                timestampResult
                )
          .Bind(_ =>
              From(
                idResult.Value,
                timestampResult.Value
                )
              );
        }

        public static Result<TimestampedId, Errors> From(
            Id id,
            Timestamp timestamp,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<TimestampedId, Errors>(
                new TimestampedId(
                  id,
                  timestamp
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Id;
            yield return Timestamp;
        }

        public static explicit operator TimestampedId(
            (Guid Id, DateTime Timestamp) timestampedId
            )
        {
            return From(timestampedId.Id, timestampedId.Timestamp).Value;
        }

        public static implicit operator (Guid, DateTime)(TimestampedId timestampedId)
        {
            return (timestampedId.Id, timestampedId.Timestamp);
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/tuples#deconstructing-user-defined-types
        public void Deconstruct(out Guid id, out DateTime timestamp)
        {
            id = Id;
            timestamp = Timestamp;
        }

        public static explicit operator TimestampedId(
            (Id Id, Timestamp Timestamp) timestampedId
            )
        {
            return From(timestampedId.Id, timestampedId.Timestamp).Value;
        }

        public static implicit operator (Id, Timestamp)(TimestampedId timestampedId)
        {
            return (timestampedId.Id, timestampedId.Timestamp);
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/tuples#deconstructing-user-defined-types
        public void Deconstruct(out Id id, out Timestamp timestamp)
        {
            id = Id;
            timestamp = Timestamp;
        }
    }
}