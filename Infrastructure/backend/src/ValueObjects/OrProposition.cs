using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class OrProposition<TVariable>
      : NAryCompoundProposition<TVariable>
    {
        public static Result<OrProposition<TVariable>, Errors> From(
            IEnumerable<Proposition<TVariable>> propositions,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<OrProposition<TVariable>, Errors>(
                new OrProposition<TVariable>(propositions)
                );
        }

        private OrProposition(
            IEnumerable<Proposition<TVariable>> propositions
            )
          : base(
              propositions,
              neutralElement: false,
              binaryOperator: (operand1, operand2) => operand1 || operand2
              )
        {
        }
    }
}