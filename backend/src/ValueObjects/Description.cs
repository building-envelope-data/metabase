using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

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

    public static Result<Description> From(string description)
    {
      description = description.Trim();

      if (description.Length == 0)
        return Result.Failure<Description>("Description is empty");

      if (description.Length >= 128)
        return Result.Failure<Description>("Description is too long");

      return Result.Ok(new Description(description));
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
