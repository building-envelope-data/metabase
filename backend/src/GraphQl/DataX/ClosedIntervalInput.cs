using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed record ClosedIntervalInput(
        double LowerBound,
        double UpperBound
        );
}
