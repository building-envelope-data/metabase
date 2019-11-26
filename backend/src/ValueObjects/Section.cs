using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class Section
    : ValueObject
  {
    public string Value { get; }

    private Section(string value)
    {
      Value = value;
    }

    public static Result<Section, IError> From(
        string section,
        IReadOnlyList<object>? path = null
        )
    {
      section = section.Trim();

      if (section.Length == 0)
        return Result.Failure<Section, IError>(
            ErrorBuilder.New()
            .SetMessage("Section is empty")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      if (section.Length > 128)
        return Result.Failure<Section, IError>(
            ErrorBuilder.New()
            .SetMessage("Section is too long")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<Section, IError>(new Section(section));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Section(string section)
    {
      return From(section).Value;
    }

    public static implicit operator string(Section section)
    {
      return section.Value;
    }
  }
}
