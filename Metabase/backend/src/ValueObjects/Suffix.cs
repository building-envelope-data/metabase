using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
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
            {
                return Result.Failure<Suffix, Errors>(
                    Errors.One(
                    message: "Suffix is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            if (suffix.Length > 10)
            {
                return Result.Failure<Suffix, Errors>(
                    Errors.One(
                    message: "Suffix is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Success<Suffix, Errors>(new Suffix(suffix));
        }

        public static Result<Suffix, Errors>? MaybeFrom(
            string? suffix,
            IReadOnlyList<object>? path = null
            )
        {
            if (suffix is null)
                return null;

            return From(suffix: suffix!, path: path);
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