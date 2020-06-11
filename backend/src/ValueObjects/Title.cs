using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Array = System.Array;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using ErrorCodes = Icon.ErrorCodes;
using IError = HotChocolate.IError;

namespace Icon.ValueObjects
{
    public sealed class Title
      : ValueObject
    {
        public string Value { get; }

        private Title(string value)
        {
            Value = value;
        }

        public static Result<Title, Errors> From(
            string title,
            IReadOnlyList<object>? path = null
            )
        {
            title = title.Trim();

            if (title.Length == 0)
                return Result.Failure<Title, Errors>(
                    Errors.One(
                    message: "Title is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            if (title.Length > 128)
                return Result.Failure<Title, Errors>(
                    Errors.One(
                    message: "Title is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Title, Errors>(new Title(title));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Title(string title)
        {
            return From(title).Value;
        }

        public static implicit operator string(Title title)
        {
            return title.Value;
        }
    }
}