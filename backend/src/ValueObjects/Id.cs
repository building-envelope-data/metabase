using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

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

    public static Result<Id, IError> From(
        Guid id,
        IReadOnlyList<object>? path = null
        )
    {
      if (id == Guid.Empty)
        return Result.Failure<Id, IError>(
            ErrorBuilder.New()
            .SetMessage("Id is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Id, IError>(new Id(id));
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
