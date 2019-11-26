using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class Description
    : ValueObject
  {
    public string Value { get; }

    private Description(string value)
    {
      Value = value;
    }

    public static Result<Description, IError> From(
        string description,
        IReadOnlyList<object>? path = null
        )
    {
      description = description.Trim();

      if (description.Length == 0)
        return Result.Failure<Description, IError>(
            ErrorBuilder.New()
            .SetMessage("Description is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (description.Length > 128)
        return Result.Failure<Description, IError>(
            ErrorBuilder.New()
            .SetMessage("Description is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Description, IError>(new Description(description));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Description(string description)
    {
      return From(description).Value;
    }

    public static implicit operator string(Description description)
    {
      return description.Value;
    }
  }
}
