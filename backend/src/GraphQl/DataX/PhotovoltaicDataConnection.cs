using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class PhotovoltaicDataConnection
        : DataConnectionBase<PhotovoltaicDataEdge, PhotovoltaicData>
    {
        public PhotovoltaicDataConnection(
            IReadOnlyList<PhotovoltaicDataEdge> edges,
            IReadOnlyList<PhotovoltaicData> nodes,
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