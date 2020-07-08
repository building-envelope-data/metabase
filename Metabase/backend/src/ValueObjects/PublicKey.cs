using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class PublicKey
      : ValueObject
    {
        public string Value { get; }

        private PublicKey(string value)
        {
            Value = value;
        }

        public static Result<PublicKey, Errors> From(
            string publicKey,
            IReadOnlyList<object>? path = null
            )
        {
            publicKey = publicKey.Trim();
            if (publicKey.Length == 0)
            {
                return Result.Failure<PublicKey, Errors>(
                    Errors.One(
                    message: "Public key is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            if (publicKey.Length > 512)
            {
                return Result.Failure<PublicKey, Errors>(
                    Errors.One(
                    message: "Public key is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Ok<PublicKey, Errors>(new PublicKey(publicKey));
        }

        public static Result<PublicKey, Errors>? MaybeFrom(
            string? publicKey,
            IReadOnlyList<object>? path = null
            )
        {
            if (publicKey is null)
                return null;

            return From(publicKey: publicKey!, path: path);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator PublicKey(string publicKey)
        {
            return From(publicKey).Value;
        }

        public static implicit operator string(PublicKey publicKey)
        {
            return publicKey.Value;
        }
    }
}