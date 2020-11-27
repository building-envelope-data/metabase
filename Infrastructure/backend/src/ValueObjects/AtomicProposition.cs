using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public abstract class AtomicProposition<TVariable>
      : Proposition<TVariable>
    {
        public TVariable Variable { get; }

        protected AtomicProposition(
            TVariable variable
            )
        {
            Variable = variable;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Variable;
        }

        protected Result<TValue, Errors> GetCastedNonNullVariableValue<TValue>(
            Func<TVariable, object?> getVariableValue
            )
        {
            var value = getVariableValue(Variable);
            if (value is null)
            {
                return Result.Failure<TValue, Errors>(
                    Errors.One(
                      message: $"The variable {Variable} has the value null",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            if (value is TValue castedValue)
            {
                return Result.Success<TValue, Errors>(castedValue);
            }
            return Result.Failure<TValue, Errors>(
                Errors.One(
                  message: $"The value {value} of the variable {Variable} is not of type {typeof(TValue)}",
                  code: ErrorCodes.InvalidValue
                  )
                );
        }
    }
}