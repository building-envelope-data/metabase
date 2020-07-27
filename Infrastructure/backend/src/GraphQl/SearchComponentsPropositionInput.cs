using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Infrastructure.GraphQl
{
    // The current GraphQL specification from June 2018, see
    // http://spec.graphql.org/June2018/
    // does not allow input types to be unions. The upcoming version will
    // probably support union input types and possible implementations can be
    // read about on
    // https://github.com/graphql/graphql-spec/blob/master/rfcs/InputUnion.md
    // The discussion that lead to that document is
    // https://github.com/graphql/graphql-spec/issues/627
    // We therefore implement propositions, also known as boolean expressions,
    // to search for components as seen below. This is the approach taken by
    // Hasura, see
    // https://hasura.io/docs/1.0/graphql/manual/api-reference/graphql-api/query.html#whereexp
    // and
    // https://hasura.io/docs/1.0/graphql/manual/queries/query-filters.html
    public abstract class SearchComponentsPropositionInput<TSubclass, TVariable>
      where TSubclass : SearchComponentsPropositionInput<TSubclass, TVariable>
    {
        public IReadOnlyList<TSubclass>? And { get; }
        public IReadOnlyList<TSubclass>? Or { get; }
        public TSubclass? Not { get; }
        public PercentagePropositionInput? GValue { get; }
        public PercentagePropositionInput? UValue { get; }
        public PercentagePropositionInput? NearnormalHemisphericalVisibleTransmittance { get; }

        protected SearchComponentsPropositionInput(
            IReadOnlyList<TSubclass>? and,
            IReadOnlyList<TSubclass>? or,
            TSubclass? not,
            PercentagePropositionInput? gValue,
            PercentagePropositionInput? uValue,
            PercentagePropositionInput? nearnormalHemisphericalVisibleTransmittance
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
          Result<ValueObjects.Proposition<TVariable>, Errors>
          Validate(
            TSubclass self,
            TVariable gValueVariable,
            TVariable uValueVariable,
            TVariable nearnormalHemisphericalVisibleTransmittanceVariable,
            IReadOnlyList<object> path
            )
        {
            var andResult =
              self.And is null
              ? null
              : (Result<ValueObjects.AndProposition<TVariable>, Errors>?)self.And.Select((proposition, index) =>
                  Validate(
                    proposition,
                    gValueVariable: gValueVariable,
                    uValueVariable: uValueVariable,
                    nearnormalHemisphericalVisibleTransmittanceVariable: nearnormalHemisphericalVisibleTransmittanceVariable,
                    path.Append("and").Append(index).ToList().AsReadOnly()
                    )
                  )
              .Combine()
              .Bind(propositions =>
                  ValueObjects.AndProposition<TVariable>.From(
                      propositions,
                      path.Append("and").ToList().AsReadOnly()
                  )
                  );
            var orResult =
              self.Or is null
              ? null
              : (Result<ValueObjects.OrProposition<TVariable>, Errors>?)self.Or.Select((proposition, index) =>
                  Validate(
                    proposition,
                    gValueVariable: gValueVariable,
                    uValueVariable: uValueVariable,
                    nearnormalHemisphericalVisibleTransmittanceVariable: nearnormalHemisphericalVisibleTransmittanceVariable,
                    path.Append("or").Append(index).ToList().AsReadOnly()
                    )
                  )
              .Combine()
              .Bind(propositions =>
                  ValueObjects.OrProposition<TVariable>.From(
                      propositions,
                      path.Append("or").ToList().AsReadOnly()
                  )
                  );
            var notResult =
              self.Not is null
              ? null
              : (Result<ValueObjects.NotProposition<TVariable>, Errors>?)Validate(
                  self.Not,
                  gValueVariable: gValueVariable,
                  uValueVariable: uValueVariable,
                  nearnormalHemisphericalVisibleTransmittanceVariable: nearnormalHemisphericalVisibleTransmittanceVariable,
                  path.Append("not").ToList().AsReadOnly()
                  )
              .Bind(proposition =>
                  ValueObjects.NotProposition<TVariable>.From(
                    proposition,
                    path.Append("not").ToList().AsReadOnly()
                    )
                  );
            var gValueResult =
              self.GValue is null
              ? null
              : (Result<ValueObjects.AndProposition<TVariable>, Errors>?)PercentagePropositionInput.Validate<TVariable>(
                  self.GValue,
                  gValueVariable,
                  path.Append("gValue").ToList().AsReadOnly()
                  );
            var uValueResult =
              self.UValue is null
              ? null
              : (Result<ValueObjects.AndProposition<TVariable>, Errors>?)PercentagePropositionInput.Validate<TVariable>(
                  self.UValue,
                  gValueVariable,
                  path.Append("uValue").ToList().AsReadOnly()
                  );
            var nearnormalHemisphericalVisibleTransmittancelueResult =
              self.NearnormalHemisphericalVisibleTransmittance is null
              ? null
              : (Result<ValueObjects.AndProposition<TVariable>, Errors>?)PercentagePropositionInput.Validate<TVariable>(
                  self.NearnormalHemisphericalVisibleTransmittance,
                  nearnormalHemisphericalVisibleTransmittanceVariable,
                  path.Append("nearnormalHemisphericalVisibleTransmittance").ToList().AsReadOnly()
                  );

            return
              Errors.CombineExistent(
                  andResult,
                  orResult,
                  notResult,
                  gValueResult,
                  uValueResult,
                  nearnormalHemisphericalVisibleTransmittancelueResult
                  )
              .Bind(_ =>
                  ValueObjects.AndProposition<TVariable>.From(
                    new ValueObjects.Proposition<TVariable>?[]
                    {
                        andResult?.Value,
                        orResult?.Value,
                        notResult?.Value,
                        gValueResult?.Value,
                        uValueResult?.Value,
                        nearnormalHemisphericalVisibleTransmittancelueResult?.Value
                    }
                    .OfType<ValueObjects.Proposition<TVariable>>(), // excludes null values
                    path
                    )
                  )
                  .Map(andProposition => (Proposition<ValueObjects.SearchComponentsVariable>)andProposition);
        }
    }
}