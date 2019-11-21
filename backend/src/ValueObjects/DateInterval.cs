using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
  public sealed class DateInterval
    : ValueObject
  {
    public DateTime? Start { get; }
    public DateTime? End { get; }

    private DateInterval(
        DateTime? start,
        DateTime? end
        )
    {
      Start = start;
      End = end;
    }

    public static Result<DateInterval> From(
        DateTime? start,
        DateTime? end
        )
    {
      if (start is null && end is null)
        return Result.Failure<DateInterval>("Start and end are both null");

      if (!(start is null) && !(end is null) && start > end)
        return Result.Failure<DateInterval>("Start lies after end");

      return Result.Ok(new DateInterval(start: start, end: end));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }

    public static explicit operator DateInterval(
        (DateTime? Start, DateTime? End) dateInterval
        )
    {
      return From(start: dateInterval.Start, end: dateInterval.End).Value;
    }

    public static implicit operator (DateTime? Start, DateTime? End)(DateInterval dateInterval)
    {
      return (Start: dateInterval.Start, End: dateInterval.End);
    }

    // https://docs.microsoft.com/en-us/dotnet/csharp/tuples#deconstructing-user-defined-types
    public void Deconstruct(out DateTime? start, out DateTime? end)
    {
        start = Start;
        end = End;
    }
  }
}
