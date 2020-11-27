using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class InClosedIntervalProposition<TVariable, TValue>
      : AtomicProposition<TVariable>
      where TValue : IComparable
    {
        public ClosedInterval<TValue> Interval { get; }

        private InClosedIntervalProposition(
            TVariable variable,
            ClosedInterval<TValue> interval
            )
          : base(variable)
        {
            Interval = interval;
        }

        public static Result<InClosedIntervalProposition<TVariable, TValue>, Errors> From(
            TVariable variable,
            ClosedInterval<TValue> interval,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<InClosedIntervalProposition<TVariable, TValue>, Errors>(
                new InClosedIntervalProposition<TVariable, TValue>(
                  variable,
                  interval
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            foreach (var component in base.GetEqualityComponents())
            {
                yield return component;
            }
            yield return Interval;
        }

        public override Result<bool, Errors> Evaluate(
            Func<TVariable, object?> getVariableValue
            )
        {
            return
              GetCastedNonNullVariableValue<TValue>(getVariableValue)
              .Map(value => Interval.Contains(value));
        }
    }
}