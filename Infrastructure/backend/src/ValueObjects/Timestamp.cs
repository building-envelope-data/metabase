using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using DateTimeKind = System.DateTimeKind;

namespace Infrastructure.ValueObjects
{
    public sealed class Timestamp
      : ValueObject
    {
        public static Timestamp Now
        {
            get { return ValueObjects.Timestamp.From(DateTime.UtcNow).Value; }
        }

        // TODO Use `NodaTime.ZonedDateTime` (or `DateTimeOffset`)? The latter is not fully time-zone aware as explained on https://docs.microsoft.com/en-us/dotnet/api/system.datetimeoffset?view=netcore-3.1 because it only stores the offset and not the time zone and is thus not aware of daylight saving time.
        public DateTime Value { get; }

        private Timestamp(DateTime value)
        {
            Value = value;
        }

        public static Result<Timestamp, Errors> From(
            DateTime timestamp,
            DateTime? now = null,
            IReadOnlyList<object>? path = null
            )
        {
            var nonNullNow = now ?? DateTime.UtcNow;
            if (timestamp.Kind != DateTimeKind.Utc)
            {
                // Note that calling
                // [`DateTime#ToUniversalTime`](https://docs.microsoft.com/en-us/dotnet/api/system.datetime.touniversaltime?view=netcore-3.1)
                // on the given timestamp to convert it to Coordinated
                // Universal Time (UTC) is in general not correct because it
                // interprets the timestamp as one in the server's locale which
                // in general differs from the user's locale in which the
                // timestamp was given.
                return Result.Failure<Timestamp, Errors>(
                    Errors.One(
                    message: $"Timestamp {timestamp} is not given in Coordinated Universal Time (UTC)",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            if (timestamp > nonNullNow)
            {
                return Result.Failure<Timestamp, Errors>(
                    Errors.One(
                    message: $"Timestamp {timestamp} is in the future where {nonNullNow} is now",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Success<Timestamp, Errors>(
                new Timestamp(timestamp)
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public string InUtcFormat()
        {
            return Value.ToString("yyyy-MM-ddTHH\\:mm\\:ss.ffffffZ");
        }

        public static explicit operator Timestamp(DateTime timestamp)
        {
            return From(timestamp).Value;
        }

        public static implicit operator DateTime(Timestamp timestamp)
        {
            return timestamp.Value;
        }

        public override string ToString()
        {
            return $"{GetType()}({Value})";
        }
    }
}