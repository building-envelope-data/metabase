using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class AndProposition<TVariable>
      : NAryCompoundProposition<TVariable>
    {
        public static Result<AndProposition<TVariable>, Errors> From(
            IEnumerable<Proposition<TVariable>> propositions,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<AndProposition<TVariable>, Errors>(
                new AndProposition<TVariable>(propositions)
                );
        }

        private AndProposition(
            IEnumerable<Proposition<TVariable>> propositions
            )
          : base(
              propositions,
              neutralElement: true,
              binaryOperator: (operand1, operand2) => operand1 && operand2
              )
        {
        }
    }
}