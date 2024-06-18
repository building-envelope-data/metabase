using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class DataConnectionIgsdb
    : DataConnectionBase<DataEdgeIgsdb, IDataIgsdb>
{
    public DataConnectionIgsdb(
        IReadOnlyList<DataEdgeIgsdb> edges,
        IReadOnlyList<IDataIgsdb> nodes,
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