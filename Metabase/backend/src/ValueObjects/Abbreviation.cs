using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class Abbreviation
      : ValueObject
    {
        public string Value { get; }

        private Abbreviation(string value)
        {
            Value = value;
        }

        public static Result<Abbreviation, Errors> From(
            string abbreviation,
            IReadOnlyList<object>? path = null
            )
        {
            abbreviation = abbreviation.Trim();

            if (abbreviation.Length == 0)
            {
                return Result.Failure<Abbreviation, Errors>(
                    Errors.One(
                    message: "Abbreviation is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            if (abbreviation.Length > 32)
            {
                return Result.Failure<Abbreviation, Errors>(
                    Errors.One(
                    message: "Abbreviation is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Ok<Abbreviation, Errors>(
                new Abbreviation(abbreviation)
                );
        }

        public static Result<Abbreviation, Errors>? MaybeFrom(
            string? abbreviation,
            IReadOnlyList<object>? path = null
            )
        {
            if (abbreviation is null)
                return null;

            return From(abbreviation: abbreviation!, path: path);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Abbreviation(string abbreviation)
        {
            return From(abbreviation).Value;
        }

        public static implicit operator string(Abbreviation abbreviation)
        {
            return abbreviation.Value;
        }
    }
}