using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
    public sealed class Year
      : ValueObject
    {
        public int Value { get; }

        private Year(int value)
        {
            Value = value;
        }

        public static Result<Year, Errors> From(
            int year,
            IReadOnlyList<object>? path = null
            )
        {
            if (year > DateTime.UtcNow.Year)
                return Result.Failure<Year, Errors>(
                    Errors.One(
                    message: "Year is in the future",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Year, Errors>(new Year(year));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Year(int year)
        {
            return From(year).Value;
        }

        public static implicit operator int(Year year)
        {
            return year.Value;
        }
    }
}