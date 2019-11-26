using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class Suffix
    : ValueObject
  {
    public string Value { get; }

    private Suffix(string value)
    {
      Value = value;
    }

    public static Result<Suffix, IError> From(
        string suffix,
        IReadOnlyList<object>? path = null
        )
    {
      suffix = suffix.Trim();

      if (suffix.Length == 0)
        return Result.Failure<Suffix, IError>(
            ErrorBuilder.New()
            .SetMessage("Suffix is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (suffix.Length > 10)
        return Result.Failure<Suffix, IError>(
            ErrorBuilder.New()
            .SetMessage("Suffix is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Suffix, IError>(new Suffix(suffix));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Suffix(string suffix)
    {
      return From(suffix).Value;
    }

    public static implicit operator string(Suffix suffix)
    {
      return suffix.Value;
    }
  }
}
