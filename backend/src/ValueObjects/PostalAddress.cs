using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class PostalAddress
    : ValueObject
  {
    public string Value { get; }

    private PostalAddress(string value)
    {
      Value = value;
    }

    public static Result<PostalAddress, IError> From(
        string postalAddress,
        IReadOnlyList<object>? path = null
        )
    {
      postalAddress = postalAddress.Trim();

      if (postalAddress.Length == 0)
        return Result.Failure<PostalAddress, IError>(
            ErrorBuilder.New()
            .SetMessage("Postal address is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (postalAddress.Length > 256)
        return Result.Failure<PostalAddress, IError>(
            ErrorBuilder.New()
            .SetMessage("Postal address is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<PostalAddress, IError>(
          new PostalAddress(postalAddress)
          );
    }

    protected override IEnumerable<object> GetEqualityComponents()
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
