using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class Name
      : ValueObject
    {
        public string Value { get; }

        private Name(string value)
        {
            Value = value;
        }

        public static Result<Name, Errors> From(
            string name,
            IReadOnlyList<object>? path = null
            )
        {
            name = name.Trim();

            if (name.Length == 0)
                return Result.Failure<Name, Errors>(
                    Errors.One(
                    message: "Name is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            if (name.Length > 128)
                return Result.Failure<Name, Errors>(
                    Errors.One(
                    message: "Name is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Name, Errors>(
                new Name(name)
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Name(string name)
        {
            return From(name).Value;
        }

        public static implicit operator string(Name name)
        {
            return name.Value;
        }
    }
}