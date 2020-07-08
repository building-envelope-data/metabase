using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class Description
      : ValueObject
    {
        public string Value { get; }

        private Description(string value)
        {
            Value = value;
        }

        public static Result<Description, Errors> From(
            string description,
            IReadOnlyList<object>? path = null
            )
        {
            description = description.Trim();

            if (description.Length == 0)
            {
                return Result.Failure<Description, Errors>(
                    Errors.One(
                    message: "Description is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            if (description.Length > 512)
            {
                return Result.Failure<Description, Errors>(
                    Errors.One(
                    message: "Description is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Ok<Description, Errors>(new Description(description));
        }

        public static Result<Description, Errors>? MaybeFrom(
            string? description,
            IReadOnlyList<object>? path = null
            )
        {
            if (description is null)
                return null;

            return From(description: description!, path: path);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Description(string description)
        {
            return From(description).Value;
        }

        public static implicit operator string(Description description)
        {
            return description.Value;
        }
    }
}