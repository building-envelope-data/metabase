using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class MainNumber
      : ValueObject
    {
        public string Value { get; }

        private MainNumber(string value)
        {
            Value = value;
        }

        public static Result<MainNumber, Errors> From(
            string mainNumber,
            IReadOnlyList<object>? path = null
            )
        {
            mainNumber = mainNumber.Trim();

            if (mainNumber.Length == 0)
                return Result.Failure<MainNumber, Errors>(
                    Errors.One(
                    message: "Main number is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            if (mainNumber.Length > 128)
                return Result.Failure<MainNumber, Errors>(
                    Errors.One(
                    message: "Main number is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<MainNumber, Errors>(new MainNumber(mainNumber));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator MainNumber(string mainNumber)
        {
            return From(mainNumber).Value;
        }

        public static implicit operator string(MainNumber mainNumber)
        {
            return mainNumber.Value;
        }
    }
}