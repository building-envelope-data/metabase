using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class NotProposition<TVariable>
      : Proposition<TVariable>
    {
        public static Result<NotProposition<TVariable>, Errors> From(
            Proposition<TVariable> proposition,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<NotProposition<TVariable>, Errors>(
                new NotProposition<TVariable>(proposition)
                );
        }

        public Proposition<TVariable> Proposition { get; }

        private NotProposition(
            Proposition<TVariable> proposition
            )
        {
            Proposition = proposition;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Proposition;
        }

        public override Result<bool, Errors> Evaluate(
            Func<TVariable, object?> getVariableValue
            )
        {
            return
              Proposition.Evaluate(getVariableValue)
              .Map(boolean => !boolean);
        }
    }
}