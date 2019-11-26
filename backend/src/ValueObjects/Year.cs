using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class Year
    : ValueObject
  {
    public int Value { get; }

    private Year(int value)
    {
      Value = value;
    }

    public static Result<Year, IError> From(
        int year,
        IReadOnlyList<object>? path = null
        )
    {
      if (year > DateTime.UtcNow.Year)
        return Result.Failure<Year, IError>(
            ErrorBuilder.New()
            .SetMessage("Year is in the future")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Year, IError>(new Year(year));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Year(int year)
    {
      return From(year).Value;
    }

    public static implicit operator int(Year year)
    {
      return year.Value;
    }
  }
}
