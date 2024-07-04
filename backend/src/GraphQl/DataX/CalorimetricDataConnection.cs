using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class CalorimetricDataConnection
    : DataConnectionBase<CalorimetricDataEdge>
{
    public CalorimetricDataConnection(
        IReadOnlyList<CalorimetricDataEdge> edges,
        uint totalCount,
        DateTime timestamp
    )
        : base(
            edges,
            totalCount,
            timestamp
        )
    {
    }
}