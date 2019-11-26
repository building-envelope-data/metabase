using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;
using Array = System.Array;

namespace Icon.ValueObjects
{
  public sealed class Abbreviation
    : ValueObject
  {
    public string Value { get; }

    private Abbreviation(string value)
    {
      Value = value;
    }

    public static Result<Abbreviation?, IError> From(
        string? abbreviation,
        IReadOnlyList<object>? path = null
        )
    {
      if (abbreviation is null)
        return Result.Ok<Abbreviation?, IError>(null);

      abbreviation = abbreviation.Trim();

      if (abbreviation.Length == 0)
        return Result.Failure<Abbreviation?, IError>(
            ErrorBuilder.New()
            .SetMessage("Abbreviation is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (abbreviation.Length > 32)
        return Result.Failure<Abbreviation?, IError>(
            ErrorBuilder.New()
            .SetMessage("Abbreviation is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Abbreviation?, IError>(
          new Abbreviation(abbreviation)
          );
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Abbreviation(string abbreviation)
    {
      return From(abbreviation).Value;
    }

    public static implicit operator string(Abbreviation abbreviation)
    {
      return abbreviation.Value;
    }
  }
}
