using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class ClosedInterval<TValue>
      : ValueObject
      where TValue : IComparable
    {
        public static Result<ClosedInterval<TValue>, Errors> From(
            TValue lowerBound,
            TValue upperBound,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<ClosedInterval<TValue>, Errors>(
                new ClosedInterval<TValue>(
                  lowerBound: lowerBound,
                  upperBound: upperBound
                  )
                );
        }

        public TValue LowerBound { get; }
        public TValue UpperBound { get; }

        private ClosedInterval(
            TValue lowerBound,
            TValue upperBound
            )
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return LowerBound;
            yield return UpperBound;
        }

        public bool Contains(TValue value)
        {
            return
              value.CompareTo(LowerBound) >= 0 &&
              value.CompareTo(UpperBound) <= 0;
        }
    }
}