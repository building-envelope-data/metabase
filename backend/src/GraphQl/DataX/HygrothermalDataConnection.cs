using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class HygrothermalDataConnection
    : DataConnectionBase<HygrothermalDataEdge>
{
    public HygrothermalDataConnection(
        IReadOnlyList<HygrothermalDataEdge> edges,
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