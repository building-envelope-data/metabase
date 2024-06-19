using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class OpticalDataConnectionIgsdb
    : DataConnectionBase<OpticalDataEdgeIgsdb>
{
    public OpticalDataConnectionIgsdb(
        IReadOnlyList<OpticalDataEdgeIgsdb> edges
    )
        : base(
            edges,
            Convert.ToUInt32(edges.Count),
            DateTime.UtcNow
        )
    {
    }
}