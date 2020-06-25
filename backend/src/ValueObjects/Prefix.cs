using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class Prefix
      : ValueObject
    {
        public string Value { get; }

        private Prefix(string value)
        {
            Value = value;
        }

        public static Result<Prefix, Errors> From(
            string prefix,
            IReadOnlyList<object>? path = null
            )
        {
            prefix = prefix.Trim();

            if (prefix.Length == 0)
                return Result.Failure<Prefix, Errors>(
                    Errors.One(
                    message: "Prefix is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            if (prefix.Length > 10)
                return Result.Failure<Prefix, Errors>(
                    Errors.One(
                    message: "Prefix is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Prefix, Errors>(new Prefix(prefix));
        }

        public static Result<Prefix, Errors>? MaybeFrom(
            string? prefix,
            IReadOnlyList<object>? path = null
            )
        {
            if (prefix is null)
                return null;

            return From(prefix: prefix!, path: path);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Prefix(string prefix)
        {
            return From(prefix).Value;
        }

        public static implicit operator string(Prefix prefix)
        {
            return prefix.Value;
        }
    }
}