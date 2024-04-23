using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class CalorimetricDataConnection
    : DataConnectionBase<CalorimetricDataEdge, CalorimetricData>
{
    public CalorimetricDataConnection(
        IReadOnlyList<CalorimetricDataEdge> edges,
        IReadOnlyList<CalorimetricData> nodes,
        uint totalCount,
        DateTime timestamp
    )
        : base(
            edges,
            nodes,
            totalCount,
            timestamp
        )
    {
    }
}