using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class GreaterThanOrEqualToProposition<TVariable, TValue>
      : AtomicProposition<TVariable>
      where TValue : IComparable
    {
        public static Result<GreaterThanOrEqualToProposition<TVariable, TValue>, Errors> From(
            TVariable variable,
            TValue value,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<GreaterThanOrEqualToProposition<TVariable, TValue>, Errors>(
                new GreaterThanOrEqualToProposition<TVariable, TValue>(
                  variable, value
                  )
                );
        }

        public TValue Value { get; }

        private GreaterThanOrEqualToProposition(
            TVariable variable,
            TValue value
            )
          : base(variable)
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            foreach (var component in base.GetEqualityComponents())
            {
                yield return component;
            }
            yield return Value;
        }

        public override Result<bool, Errors> Evaluate(
            Func<TVariable, object?> getVariableValue
            )
        {
            return
              GetCastedNonNullVariableValue<TValue>(
                  getVariableValue
                  )
              .Map(value =>
                  value.CompareTo(Value) >= 0
                  );
        }
    }
}