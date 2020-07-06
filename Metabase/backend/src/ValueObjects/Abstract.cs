using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class Abstract
      : ValueObject
    {
        public string Value { get; }

        private Abstract(string value)
        {
            Value = value;
        }

        public static Result<Abstract, Errors> From(
            string @abstract,
            IReadOnlyList<object>? path = null
            )
        {
            @abstract = @abstract.Trim();

            if (@abstract.Length == 0)
                return Result.Failure<Abstract, Errors>(
                    Errors.One(
                    message: "Abstract is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            if (@abstract.Length > 128)
                return Result.Failure<Abstract, Errors>(
                    Errors.One(
                    message: "Abstract is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Abstract, Errors>(new Abstract(@abstract));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Abstract(string @abstract)
        {
            return From(@abstract).Value;
        }

        public static implicit operator string(Abstract @abstract)
        {
            return @abstract.Value;
        }
    }
}