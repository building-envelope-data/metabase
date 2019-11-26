using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class MainNumber
    : ValueObject
  {
    public string Value { get; }

    private MainNumber(string value)
    {
      Value = value;
    }

    public static Result<MainNumber, IError> From(
        string mainNumber,
        IReadOnlyList<object>? path = null
        )
    {
      mainNumber = mainNumber.Trim();

      if (mainNumber.Length == 0)
        return Result.Failure<MainNumber, IError>(
            ErrorBuilder.New()
            .SetMessage("Main number is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (mainNumber.Length > 128)
        return Result.Failure<MainNumber, IError>(
            ErrorBuilder.New()
            .SetMessage("Main number is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<MainNumber, IError>(new MainNumber(mainNumber));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator MainNumber(string mainNumber)
    {
      return From(mainNumber).Value;
    }

    public static implicit operator string(MainNumber mainNumber)
    {
      return mainNumber.Value;
    }
  }
}
