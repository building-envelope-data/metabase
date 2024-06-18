using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class OpticalDataConnectionIgsdb
    : DataConnectionBase<OpticalDataEdgeIgsdb, OpticalDataIgsdb>
{
    public OpticalDataConnectionIgsdb(
        IReadOnlyList<OpticalDataEdgeIgsdb> edges,
        IReadOnlyList<OpticalDataIgsdb> nodes,
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