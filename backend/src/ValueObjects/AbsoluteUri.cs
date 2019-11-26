using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
  public sealed class AbsoluteUri
    : ValueObject
  {
    public Uri Value { get; }

    private AbsoluteUri(Uri value)
    {
      Value = value;
    }

    public static Result<AbsoluteUri, IError> From(
        Uri uri,
        IReadOnlyList<object>? path = null
        )
    {
      if (!uri.IsAbsoluteUri)
        return Result.Failure<AbsoluteUri, IError>(
            ErrorBuilder.New()
            .SetMessage("Uri is not absolute")
            .SetCode(ErrorCodes.InvalidValue)
            .SetPath(path)
            .Build()
            );

      return Result.Ok<AbsoluteUri, IError>(new AbsoluteUri(uri));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator AbsoluteUri(Uri absoluteUri)
    {
      return From(absoluteUri).Value;
    }

    public static implicit operator Uri(AbsoluteUri absoluteUri)
    {
      return absoluteUri.Value;
    }
  }
}
