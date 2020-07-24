using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.GraphQl
{
    public sealed class SearchComponentsPropositionInput
      : Infrastructure.GraphQl.SearchComponentsPropositionInput<SearchComponentsPropositionInput, ValueObjects.SearchComponentsVariable>
    {
        public SearchComponentsPropositionInput(
            IReadOnlyList<SearchComponentsPropositionInput> and,
            IReadOnlyList<SearchComponentsPropositionInput> or,
            SearchComponentsPropositionInput not,
            PercentagePropositionInput gValue,
            PercentagePropositionInput uValue,
            PercentagePropositionInput nearnormalHemisphericalVisibleTransmittance
            )
          : base(
              and: and,
              or: or,
              not: not,
              gValue: gValue,
              uValue: uValue,
              nearnormalHemisphericalVisibleTransmittance: nearnormalHemisphericalVisibleTransmittance
              )
        {
        }

        public static
          Result<AndProposition<ValueObjects.SearchComponentsVariable>, Errors>
          Validate(
            SearchComponentsPropositionInput self,
            IReadOnlyList<object> path
            )
        {
            return
              Infrastructure.GraphQl.SearchComponentsPropositionInput<SearchComponentsPropositionInput, ValueObjects.SearchComponentsVariable>.Validate(
                self,
                gValueVariable: ValueObjects.SearchComponentsVariable.G_VALUE,
                uValueVariable: ValueObjects.SearchComponentsVariable.U_VALUE,
                nearnormalHemisphericalVisibleTransmittanceVariable: ValueObjects.SearchComponentsVariable.NEARNORMAL_HEMISPHERICAL_VISIBLE_TRANSMITTANCE,
                path: path
                );
        }
    }
}