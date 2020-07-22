using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Infrastructure.GraphQl
{
    public sealed class ClosedIntervalInput
    {
        public double LowerBound { get; }
        public double UpperBound { get; }

        public ClosedIntervalInput(
            double lowerBound,
            double upperBound
            )
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public static
          Result<ValueObjects.ClosedInterval<ValueObjects.Percentage>, Errors>
          Validate(
            ClosedIntervalInput self,
            IReadOnlyList<object> path
            )
        {
            var lowerBoundResult = ValueObjects.Percentage.From(
                  self.LowerBound,
                  path.Append("lowerBound").ToList().AsReadOnly()
                  );
            var upperBoundResult = ValueObjects.Percentage.From(
                  self.UpperBound,
                  path.Append("upperBound").ToList().AsReadOnly()
                  );

            return
              Errors.Combine(
                  lowerBoundResult,
                  upperBoundResult
                  )
              .Bind(_ =>
                  ValueObjects.ClosedInterval<ValueObjects.Percentage>.From(
                    lowerBoundResult.Value,
                    upperBoundResult.Value,
                    path
                    )
                  );
        }
    }
}