using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class GeometricDataConnection
    : DataConnectionBase<GeometricDataEdge>
{
    public GeometricDataConnection(
        IReadOnlyList<GeometricDataEdge> edges,
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