using DateTime = System.DateTime;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
    public sealed class Timestamp
      : ValueObject
    {
        public static Timestamp Now
        {
            get { return ValueObjects.Timestamp.From(DateTime.UtcNow).Value; }
        }

        // TODO Use `NodaTime.ZonedDateTime`
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
            if (timestamp > nonNullNow)
                return Result.Failure<Timestamp, Errors>(
                    Errors.One(
                    message: $"Timestamp {timestamp} is in the future where {nonNullNow} is now",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Timestamp, Errors>(
                new Timestamp(timestamp)
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Timestamp(DateTime timestamp)
        {
            return From(timestamp).Value;
        }

        public static implicit operator DateTime(Timestamp timestamp)
        {
            return timestamp.Value;
        }
    }
}