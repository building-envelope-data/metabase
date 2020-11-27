using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class EqualToProposition<TVariable, TValue>
      : AtomicProposition<TVariable>
    {
        public static Result<EqualToProposition<TVariable, TValue>, Errors> From(
            TVariable variable,
            TValue value,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<EqualToProposition<TVariable, TValue>, Errors>(
                new EqualToProposition<TVariable, TValue>(
                  variable, value
                  )
                );
        }

        public TValue Value { get; }

        private EqualToProposition(
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
                  EqualityComparer<TValue>.Default.Equals(
                    Value, value
                    )
                  );
        }
    }
}