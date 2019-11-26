using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class Title
    : ValueObject
  {
    public string Value { get; }

    private Title(string value)
    {
      Value = value;
    }

    public static Result<Title, IError> From(
        string title,
        IReadOnlyList<object>? path = null
        )
    {
      title = title.Trim();

      if (title.Length == 0)
        return Result.Failure<Title, IError>(
            ErrorBuilder.New()
            .SetMessage("Title is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (title.Length > 128)
        return Result.Failure<Title, IError>(
            ErrorBuilder.New()
            .SetMessage("Title is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Title, IError>(new Title(title));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Title(string title)
    {
      return From(title).Value;
    }

    public static implicit operator string(Title title)
    {
      return title.Value;
    }
  }
}
