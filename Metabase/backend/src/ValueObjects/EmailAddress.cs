using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class EmailAddress
      : ValueObject
    {
        public string Value { get; }

        private EmailAddress(string value)
        {
            Value = value;
        }

        public static Result<EmailAddress, Errors> From(
            string emailAddress,
            IReadOnlyList<object>? path = null
            )
        {
            emailAddress = emailAddress.Trim();

            if (emailAddress.Length == 0)
            {
                return Result.Failure<EmailAddress, Errors>(
                    Errors.One(
                    message: "Email address is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            if (!Regex.IsMatch(emailAddress, @"^(.+)@(.+)$"))
            {
                return Result.Failure<EmailAddress, Errors>(
                    Errors.One(
                    message: "Email address is invalid",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Ok<EmailAddress, Errors>(new EmailAddress(emailAddress));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator EmailAddress(string emailAddress)
        {
            return From(emailAddress).Value;
        }

        public static implicit operator string(EmailAddress emailAddress)
        {
            return emailAddress.Value;
        }
    }
}