using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;
/* using DateInterval = NodaTime.DateInterval; */

namespace Icon.ValueObjects
{
    public sealed class DateInterval
      : ValueObject
    {
        // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment. We should also have a date with proper time zone support like `NodaTime.ZonedDateTime`. We could also use `NodaTime.DateInterval`
        public DateTime? Start { get; }
        public DateTime? End { get; }

        private DateInterval(
            DateTime? start,
            DateTime? end
            )
        {
            Start = start;
            End = end;
        }

        public static Result<DateInterval, Errors> From(
            DateTime? start,
            DateTime? end,
            IReadOnlyList<object>? path = null
            )
        {
            if (start is null && end is null)
                return Result.Failure<DateInterval, Errors>(
                    Errors.One(
                    message: "Start and end are both unspecified",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            if (!(start is null) && !(end is null) && start > end)
                return Result.Failure<DateInterval, Errors>(
                    Errors.One(
                    message: "Start lies after end",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<DateInterval, Errors>(
                new DateInterval(
                  start: start,
                  end: end
                  )
                );
        }

        public static Result<DateInterval, Errors>? MaybeFrom(
            DateTime? start,
            DateTime? end,
            IReadOnlyList<object>? path = null
            )
        {
            if (start is null && end is null)
                return null;

            return From(start: start, end: end, path: path);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Start;
            yield return End;
        }

        public static explicit operator DateInterval(
            (DateTime? Start, DateTime? End) dateInterval
            )
        {
            return From(start: dateInterval.Start, end: dateInterval.End).Value;
        }

        public static implicit operator (DateTime? Start, DateTime? End)(DateInterval dateInterval)
        {
            return (Start: dateInterval.Start, End: dateInterval.End);
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/tuples#deconstructing-user-defined-types
        public void Deconstruct(out DateTime? start, out DateTime? end)
        {
            start = Start;
            end = End;
        }
    }
}