using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class HygrothermalDataConnection
    : DataConnectionBase<HygrothermalDataEdge, HygrothermalData>
    {
        public HygrothermalDataConnection(
            IReadOnlyList<HygrothermalDataEdge> edges,
            IReadOnlyList<HygrothermalData> nodes,
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
