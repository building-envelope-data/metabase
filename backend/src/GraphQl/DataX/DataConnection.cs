using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class DataConnection
        : DataConnectionBase<DataEdge, IData>
    {
        public DataConnection(
            IReadOnlyList<DataEdge> edges,
            IReadOnlyList<IData> nodes,
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
}