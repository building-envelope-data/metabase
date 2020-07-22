using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Infrastructure.GraphQl
{
    public sealed class PercentagePropositionInput
    {
        public double EqualTo { get; }
        public double GreaterThanOrEqualTo { get; }
        public double LessThanOrEqualTo { get; }
        public ClosedIntervalInput InClosedInterval { get; }

        public PercentagePropositionInput(
            double equalTo,
            double greaterThanOrEqualTo,
            double lessThanOrEqualTo,
            ClosedIntervalInput inClosedInterval
            )
        {
            EqualTo = equalTo;
            GreaterThanOrEqualTo = greaterThanOrEqualTo;
            LessThanOrEqualTo = lessThanOrEqualTo;
            InClosedInterval = inClosedInterval;
        }

        public static
          Result<ValueObjects.AndProposition<ValueObjects.SearchComponentsVariable>, Errors>
          Validate(
            PercentagePropositionInput self,
            ValueObjects.SearchComponentsVariable variable,
            IReadOnlyList<object> path
            )
        {
            var equalToResult = ValueObjects.Percentage.From(
                self.EqualTo,
                path.Append("equalTo").ToList().AsReadOnly()
                )
              .Bind(percentage =>
                  ValueObjects.EqualToProposition<ValueObjects.SearchComponentsVariable, ValueObjects.Percentage>.From(
                    variable,
                    percentage,
                    path.Append("equalTo").ToList().AsReadOnly()
                    )
                  );
            var greaterThanOrEqualToResult = ValueObjects.Percentage.From(
                self.GreaterThanOrEqualTo,
                path.Append("greaterThanOrEqualTo").ToList().AsReadOnly()
                )
              .Bind(percentage =>
                  ValueObjects.GreaterThanOrEqualToProposition<ValueObjects.SearchComponentsVariable, ValueObjects.Percentage>.From(
                    variable,
                    percentage,
                    path.Append("greaterThanOrEqualTo").ToList().AsReadOnly()
                    )
                  );
            var lessThanOrEqualToResult = ValueObjects.Percentage.From(
                self.LessThanOrEqualTo,
                path.Append("lessThanOrLessThanOrEqualTo").ToList().AsReadOnly()
                )
              .Bind(percentage =>
                  ValueObjects.LessThanOrEqualToProposition<ValueObjects.SearchComponentsVariable, ValueObjects.Percentage>.From(
                    variable,
                    percentage,
                    path.Append("lessThanOrEqualTo").ToList().AsReadOnly()
                    )
                  );
            var inClosedIntervalResult = ClosedIntervalInput.Validate(
                self.InClosedInterval,
                path.Append("inClosedInterval").ToList().AsReadOnly()
                )
              .Bind(closedInterval =>
                  ValueObjects.InClosedIntervalProposition<ValueObjects.SearchComponentsVariable, ValueObjects.Percentage>.From(
                    variable,
                    closedInterval,
                    path.Append("inClosedInterval").ToList().AsReadOnly()
                    )
                  );

            return
              Errors.Combine(
                  equalToResult,
                  greaterThanOrEqualToResult,
                  lessThanOrEqualToResult,
                  inClosedIntervalResult
                  )
              .Bind(_ =>
                  ValueObjects.AndProposition<ValueObjects.SearchComponentsVariable>.From(
                    new ValueObjects.Proposition<ValueObjects.SearchComponentsVariable>[]
                    {
                        equalToResult.Value,
                        greaterThanOrEqualToResult.Value,
                        lessThanOrEqualToResult.Value,
                        inClosedIntervalResult.Value
                    },
                    path
                    )
                  );
        }
    }
}