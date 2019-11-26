using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class PhoneNumber
    : ValueObject
  {
    public string Value { get; }

    private PhoneNumber(string value)
    {
      Value = value;
    }

    public static Result<PhoneNumber, IError> From(
        string phoneNumber,
        IReadOnlyList<object>? path = null
        )
    {
      phoneNumber = phoneNumber.Trim();

      if (phoneNumber.Length == 0)
        return Result.Failure<PhoneNumber, IError>(
            ErrorBuilder.New()
            .SetMessage("Phone number is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (phoneNumber.Length > 128)
        return Result.Failure<PhoneNumber, IError>(
            ErrorBuilder.New()
            .SetMessage("Phone number is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<PhoneNumber, IError>(
          new PhoneNumber(phoneNumber)
          );
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator PhoneNumber(string phoneNumber)
    {
      return From(phoneNumber).Value;
    }

    public static implicit operator string(PhoneNumber phoneNumber)
    {
      return phoneNumber.Value;
    }
  }
}
