using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

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

    public static Result<EmailAddress> From(string emailAddress)
    {
      emailAddress = emailAddress.Trim();

      if (emailAddress.Length == 0)
        return Result.Failure<EmailAddress>("Email address is empty");

      if (!Regex.IsMatch(emailAddress, @"^(.+)@(.+)$"))
        return Result.Failure<EmailAddress>("Email address is invalid");

      return Result.Ok(new EmailAddress(emailAddress));
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
