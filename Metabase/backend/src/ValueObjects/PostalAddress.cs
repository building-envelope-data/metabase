using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class PostalAddress
      : ValueObject
    {
        public string Value { get; }

        private PostalAddress(string value)
        {
            Value = value;
        }

        public static Result<PostalAddress, Errors> From(
            string postalAddress,
            IReadOnlyList<object>? path = null
            )
        {
            postalAddress = postalAddress.Trim();

            if (postalAddress.Length == 0)
            {
                return Result.Failure<PostalAddress, Errors>(
                    Errors.One(
                    message: "Postal address is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            if (postalAddress.Length > 256)
            {
                return Result.Failure<PostalAddress, Errors>(
                    Errors.One(
                    message: "Postal address is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Success<PostalAddress, Errors>(
                new PostalAddress(postalAddress)
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator PostalAddress(string postalAddress)
        {
            return From(postalAddress).Value;
        }

        public static implicit operator string(PostalAddress postalAddress)
        {
            return postalAddress.Value;
        }
    }
}