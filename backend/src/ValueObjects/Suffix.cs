using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
    public sealed class Suffix
      : ValueObject
    {
        public string Value { get; }

        private Suffix(string value)
        {
            Value = value;
        }

        public static Result<Suffix, Errors> From(
            string suffix,
            IReadOnlyList<object>? path = null
            )
        {
            suffix = suffix.Trim();

            if (suffix.Length == 0)
                return Result.Failure<Suffix, Errors>(
                    Errors.One(
                    message: "Suffix is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            if (suffix.Length > 10)
                return Result.Failure<Suffix, Errors>(
                    Errors.One(
                    message: "Suffix is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Suffix, Errors>(new Suffix(suffix));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Suffix(string suffix)
        {
            return From(suffix).Value;
        }

        public static implicit operator string(Suffix suffix)
        {
            return suffix.Value;
        }
    }
}