using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class OpticalDataConnection
    : DataConnectionBase<OpticalDataEdge, OpticalData>
    {
        public OpticalDataConnection(
            IReadOnlyList<OpticalDataEdge> edges,
            IReadOnlyList<OpticalData> nodes,
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
