using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class PublicKey
    : ValueObject
  {
    public string Value { get; }

    private PublicKey(string value)
    {
      Value = value;
    }

    public static Result<PublicKey, IError> From(
        string publicKey,
        IReadOnlyList<object>? path = null
        )
    {
      publicKey = publicKey.Trim();

      if (publicKey.Length == 0)
        return Result.Failure<PublicKey, IError>(
            ErrorBuilder.New()
            .SetMessage("Public key is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (publicKey.Length > 512)
        return Result.Failure<PublicKey, IError>(
            ErrorBuilder.New()
            .SetMessage("Public key is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<PublicKey, IError>(new PublicKey(publicKey));
    }

    protected override IEnumerable<object> GetEqualityComponents()
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
