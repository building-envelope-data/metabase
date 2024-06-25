using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class DataConnectionIgsdb
    : DataConnectionBase<DataEdgeIgsdb>
{
    public DataConnectionIgsdb(
        IReadOnlyList<DataEdgeIgsdb> edges
    )
        : base(
            edges,
            Convert.ToUInt32(edges.Count),
            DateTime.UtcNow
        )
    {
    }
}