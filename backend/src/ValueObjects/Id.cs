using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
  public sealed class Id
    : ValueObject
  {
    public Guid Value { get; }

    private Id(Guid value)
    {
      Value = value;
    }

    public static Result<Id> From(Guid id)
    {
      if (id == Guid.Empty)
        return Result.Failure<Id>("Id is empty");

      return Result.Ok(new Id(id));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Id(Guid id)
    {
      return From(id).Value;
    }

    public static implicit operator Guid(Id id)
    {
      return id.Value;
    }
  }
}
