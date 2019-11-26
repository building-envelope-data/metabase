using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class Prefix
    : ValueObject
  {
    public string Value { get; }

    private Prefix(string value)
    {
      Value = value;
    }

    public static Result<Prefix, IError> From(
        string prefix,
        IReadOnlyList<object>? path = null
        )
    {
      prefix = prefix.Trim();

      if (prefix.Length == 0)
        return Result.Failure<Prefix, IError>(
            ErrorBuilder.New()
            .SetMessage("Prefix is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (prefix.Length > 10)
        return Result.Failure<Prefix, IError>(
            ErrorBuilder.New()
            .SetMessage("Prefix is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Prefix, IError>(new Prefix(prefix));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Prefix(string prefix)
    {
      return From(prefix).Value;
    }

    public static implicit operator string(Prefix prefix)
    {
      return prefix.Value;
    }
  }
}
