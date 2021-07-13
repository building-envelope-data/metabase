using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record FloatPropositionInput(
        double? EqualTo,
        double? GreaterThanOrEqualTo,
        ClosedIntervalInput? InClosedInterval,
        double? LessThanOrEqualTo
        );
}
