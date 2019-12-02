using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
    public sealed class AbsoluteUri
      : ValueObject
    {
        public Uri Value { get; }

        private AbsoluteUri(Uri value)
        {
            Value = value;
        }

        public static Result<AbsoluteUri, Errors> From(
            Uri uri,
            IReadOnlyList<object>? path = null
            )
        {
            if (!uri.IsAbsoluteUri)
                return Result.Failure<AbsoluteUri, Errors>(
                    Errors.One(
                    message: "Uri is not absolute",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<AbsoluteUri, Errors>(new AbsoluteUri(uri));
        }

        public static Result<AbsoluteUri, Errors>? MaybeFrom(
            Uri? uri,
            IReadOnlyList<object>? path = null
            )
        {
            if (uri is null)
                return null;

            return From(uri: uri!, path: path);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator AbsoluteUri(Uri absoluteUri)
        {
            return From(absoluteUri).Value;
        }

        public static implicit operator Uri(AbsoluteUri absoluteUri)
        {
            return absoluteUri.Value;
        }
    }
}