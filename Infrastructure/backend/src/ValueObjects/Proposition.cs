using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;

namespace Infrastructure.ValueObjects
{
    public abstract class Proposition<TVariable>
      : ValueObject
    {
        protected Proposition()
        {
        }

        public abstract Result<bool, Errors> Evaluate(
            Func<TVariable, object?> getVariableValue
            );
    }
}