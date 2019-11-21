using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

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

    public static Result<AbsoluteUri> From(Uri uri)
    {
      if (!uri.IsAbsoluteUri)
        return Result.Failure<AbsoluteUri>("Uri is not absolute");

      return Result.Ok(new AbsoluteUri(uri));
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
