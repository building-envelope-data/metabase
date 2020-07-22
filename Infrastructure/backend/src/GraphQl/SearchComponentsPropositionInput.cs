using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Infrastructure.GraphQl
{
    public sealed class SearchComponentsPropositionInput
    {
        public IReadOnlyList<SearchComponentsPropositionInput> And { get; }
        public IReadOnlyList<SearchComponentsPropositionInput> Or { get; }
        public SearchComponentsPropositionInput Not { get; }
        public PercentagePropositionInput GValue { get; }
        public PercentagePropositionInput UValue { get; }
        public PercentagePropositionInput NearnormalHemisphericalVisibleTransmittance { get; }

        public SearchComponentsPropositionInput(
            IReadOnlyList<SearchComponentsPropositionInput> and,
            IReadOnlyList<SearchComponentsPropositionInput> or,
            SearchComponentsPropositionInput not,
            PercentagePropositionInput gValue,
            PercentagePropositionInput uValue,
            PercentagePropositionInput nearnormalHemisphericalVisibleTransmittance
            )
        {
            And = and;
            Or = or;
            Not = not;
            GValue = gValue;
            UValue = uValue;
            NearnormalHemisphericalVisibleTransmittance = nearnormalHemisphericalVisibleTransmittance;
        }

        public static
          Result<ValueObjects.AndProposition<ValueObjects.SearchComponentsVariable>, Errors>
          Validate(
            SearchComponentsPropositionInput self,
            IReadOnlyList<object> path
            )
        {
            var andResult =
              self.And.Select((proposition, index) =>
                  Validate(
                    proposition,
                    path.Append("and").Append(index).ToList().AsReadOnly()
                    )
                  )
              .Combine()
              .Bind(propositions =>
                  ValueObjects.AndProposition<ValueObjects.SearchComponentsVariable>.From(
                      propositions,
                      path.Append("and").ToList().AsReadOnly()
                  )
                  );
            var orResult =
              self.Or.Select((proposition, index) =>
                  Validate(
                    proposition,
                    path.Append("or").Append(index).ToList().AsReadOnly()
                    )
                  )
              .Combine()
              .Bind(propositions =>
                  ValueObjects.OrProposition<ValueObjects.SearchComponentsVariable>.From(
                      propositions,
                      path.Append("or").ToList().AsReadOnly()
                  )
                  );
            var notResult =
              Validate(
                  self.Not,
                  path.Append("not").ToList().AsReadOnly()
                  )
              .Bind(proposition =>
                  ValueObjects.NotProposition<ValueObjects.SearchComponentsVariable>.From(
                    proposition,
                    path.Append("not").ToList().AsReadOnly()
                    )
                  );
            var gValueResult =
              PercentagePropositionInput.Validate(
                  self.GValue,
                  ValueObjects.SearchComponentsVariable.G_VALUE,
                  path.Append("gValue").ToList().AsReadOnly()
                  );
            var uValueResult =
              PercentagePropositionInput.Validate(
                  self.UValue,
                  ValueObjects.SearchComponentsVariable.G_VALUE,
                  path.Append("uValue").ToList().AsReadOnly()
                  );
            var nearnormalHemisphericalVisibleTransmittancelueResult =
              PercentagePropositionInput.Validate(
                  self.NearnormalHemisphericalVisibleTransmittance,
                  ValueObjects.SearchComponentsVariable.NEARNORMAL_HEMISPHERICAL_VISIBLE_TRANSMITTANCE,
                  path.Append("nearnormalHemisphericalVisibleTransmittance").ToList().AsReadOnly()
                  );

            return
              Errors.Combine(
                  andResult,
                  orResult,
                  notResult,
                  gValueResult,
                  uValueResult,
                  nearnormalHemisphericalVisibleTransmittancelueResult
                  )
              .Bind(_ =>
                  ValueObjects.AndProposition<ValueObjects.SearchComponentsVariable>.From(
                    new ValueObjects.Proposition<ValueObjects.SearchComponentsVariable>[] {
                        andResult.Value,
                        orResult.Value,
                        notResult.Value,
                        gValueResult.Value,
                        uValueResult.Value,
                        nearnormalHemisphericalVisibleTransmittancelueResult.Value
                    },
                    path
                    )
                  );
        }
    }
}