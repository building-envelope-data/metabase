using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record ClosedIntervalInput(
        double LowerBound,
        double UpperBound
        );
}
