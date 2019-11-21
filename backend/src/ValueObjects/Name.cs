using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

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

    public static Result<Name> From(string name)
    {
      name = name.Trim();

      if (name.Length == 0)
        return Result.Failure<Name>("Name is empty");

      if (name.Length >= 128)
        return Result.Failure<Name>("Name is too long");

      return Result.Ok(new Name(name));
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
