using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;
using Array = System.Array;

namespace Icon.ValueObjects
{
  public sealed class EmailAddress
    : ValueObject
  {
    public string Value { get; }

    private EmailAddress(string value)
    {
      Value = value;
    }

    public static Result<EmailAddress, IError> From(
        string emailAddress,
        IReadOnlyList<object>? path = null
        )
    {
      emailAddress = emailAddress.Trim();

      if (emailAddress.Length == 0)
        return Result.Failure<EmailAddress, IError>(
            ErrorBuilder.New()
            .SetMessage("Email address is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (!Regex.IsMatch(emailAddress, @"^(.+)@(.+)$"))
        return Result.Failure<EmailAddress, IError>(
            ErrorBuilder.New()
            .SetMessage("Email address is invalid")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<EmailAddress, IError>(new EmailAddress(emailAddress));
    }

    protected override IEnumerable<object> GetEqualityComponents()
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
