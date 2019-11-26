using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class Abstract
    : ValueObject
  {
    public string Value { get; }

    private Abstract(string value)
    {
      Value = value;
    }

    public static Result<Abstract, IError> From(
        string @abstract,
        IReadOnlyList<object>? path = null
        )
    {
      @abstract = @abstract.Trim();

      if (@abstract.Length == 0)
        return Result.Failure<Abstract, IError>(
            ErrorBuilder.New()
            .SetMessage("Abstract is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (@abstract.Length > 128)
        return Result.Failure<Abstract, IError>(
            ErrorBuilder.New()
            .SetMessage("Abstract is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Abstract, IError>(new Abstract(@abstract));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Abstract(string @abstract)
    {
      return From(@abstract).Value;
    }

    public static implicit operator string(Abstract @abstract)
    {
      return @abstract.Value;
    }
  }
}
