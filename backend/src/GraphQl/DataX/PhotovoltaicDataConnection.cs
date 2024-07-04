using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class PhotovoltaicDataConnection
    : DataConnectionBase<PhotovoltaicDataEdge>
{
    public PhotovoltaicDataConnection(
        IReadOnlyList<PhotovoltaicDataEdge> edges,
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