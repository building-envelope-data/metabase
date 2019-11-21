using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

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

    public static Result<Abbreviation> From(string abbreviation)
    {
      abbreviation = abbreviation.Trim();

      if (abbreviation.Length == 0)
        return Result.Failure<Abbreviation>("Abbreviation is empty");

      if (abbreviation.Length >= 32)
        return Result.Failure<Abbreviation>("Abbreviation is too long");

      return Result.Ok(new Abbreviation(abbreviation));
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
