using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class Name
    : ValueObject
  {
    public string Value { get; }

    private Name(string value)
    {
      Value = value;
    }

    public static Result<Name, IError> From(
        string name,
        IReadOnlyList<object>? path = null
        )
    {
      name = name.Trim();

      if (name.Length == 0)
        return Result.Failure<Name, IError>(
            ErrorBuilder.New()
            .SetMessage("Name is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (name.Length > 128)
        return Result.Failure<Name, IError>(
            ErrorBuilder.New()
            .SetMessage("Name is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Name, IError>(
          new Name(name)
          );
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Name(string name)
    {
      return From(name).Value;
    }

    public static implicit operator string(Name name)
    {
      return name.Value;
    }
  }
}
