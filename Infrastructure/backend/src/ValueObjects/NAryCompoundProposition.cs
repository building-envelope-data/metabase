using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public abstract class NAryCompoundProposition<TVariable>
      : CompoundProposition<TVariable>
    {
        public IEnumerable<Proposition<TVariable>> Propositions { get; }
        private readonly bool _neutralElement;
        private readonly Func<bool, bool, bool> _binaryOperator;

        protected NAryCompoundProposition(
            IEnumerable<Proposition<TVariable>> propositions,
            bool neutralElement,
            Func<bool, bool, bool> binaryOperator
            )
        {
            Propositions = propositions;
            _neutralElement = neutralElement;
            _binaryOperator = binaryOperator;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            foreach (var proposition in Propositions)
            {
                yield return proposition;
            }
        }

        public override Result<bool, Errors> Evaluate(
            Func<TVariable, object?> getVariableValue
            )
        {
            return
              Propositions.Select(proposition =>
                  proposition.Evaluate(getVariableValue)
                  )
              .Combine()
              .Map(booleans =>
                  booleans.Aggregate(
                    _neutralElement,
                    _binaryOperator
                    )
                  );
        }
    }
}