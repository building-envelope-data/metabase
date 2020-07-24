using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class SearchComponentsPropositionInput
      : Infrastructure.GraphQl.SearchComponentsPropositionInput<SearchComponentsPropositionInput, ValueObjects.SearchComponentsVariable>
    {
        public StringPropositionInput Name { get; }
        public StringPropositionInput Abbreviation { get; }
        public StringPropositionInput Description { get; }
        // TODO
        /* public DateIntervalPropositionInput<ValueObjects.SearchComponentsVariable> Availability { get; } */
        /* public Categories { get; } */

        public SearchComponentsPropositionInput(
            IReadOnlyList<SearchComponentsPropositionInput> and,
            IReadOnlyList<SearchComponentsPropositionInput> or,
            SearchComponentsPropositionInput not,
            PercentagePropositionInput gValue,
            PercentagePropositionInput uValue,
            PercentagePropositionInput nearnormalHemisphericalVisibleTransmittance,
            StringPropositionInput name,
            StringPropositionInput abbreviation,
            StringPropositionInput description
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
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
        }

        public static
          Result<AndProposition<ValueObjects.SearchComponentsVariable>, Errors>
          Validate(
            SearchComponentsPropositionInput self,
            IReadOnlyList<object> path
            )
        {
            var baseResult =
              Infrastructure.GraphQl.SearchComponentsPropositionInput<SearchComponentsPropositionInput, ValueObjects.SearchComponentsVariable>.Validate(
                self,
                gValueVariable: ValueObjects.SearchComponentsVariable.G_VALUE,
                uValueVariable: ValueObjects.SearchComponentsVariable.U_VALUE,
                nearnormalHemisphericalVisibleTransmittanceVariable: ValueObjects.SearchComponentsVariable.NEARNORMAL_HEMISPHERICAL_VISIBLE_TRANSMITTANCE,
                path: path
                );
            var nameResult =
              StringPropositionInput.Validate<ValueObjects.SearchComponentsVariable>(
                  self.Name,
                  ValueObjects.SearchComponentsVariable.NAME,
                  path.Append("name").ToList().AsReadOnly()
                  );
            var abbreviationResult =
              StringPropositionInput.Validate<ValueObjects.SearchComponentsVariable>(
                  self.Abbreviation,
                  ValueObjects.SearchComponentsVariable.ABBREVIATION,
                  path.Append("abbreviation").ToList().AsReadOnly()
                  );
            var descriptionResult =
              StringPropositionInput.Validate<ValueObjects.SearchComponentsVariable>(
                  self.Description,
                  ValueObjects.SearchComponentsVariable.DESCRIPTION,
                  path.Append("description").ToList().AsReadOnly()
                  );

            return
              Errors.Combine(
                  baseResult,
                  nameResult,
                  abbreviationResult,
                  descriptionResult
                  )
              .Bind(_ =>
                  AndProposition<ValueObjects.SearchComponentsVariable>.From(
                    new Proposition<ValueObjects.SearchComponentsVariable>[]
                    {
                        baseResult.Value,
                        nameResult.Value,
                        abbreviationResult.Value,
                        descriptionResult.Value
                    },
                    path
                    )
                  );
        }
    }
}