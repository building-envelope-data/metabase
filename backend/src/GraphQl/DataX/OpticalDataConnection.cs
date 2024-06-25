using System;
using System.Collections.Generic;
using System.Linq;

namespace Metabase.GraphQl.DataX;

public sealed class OpticalDataConnection
    : DataConnectionBase<OpticalDataEdge>
{
    internal static OpticalDataConnection? From(OpticalDataConnectionIgsdb? allOpticalData)
    {
        if (allOpticalData is null) {
            return null;
        }
        return new OpticalDataConnection(
            allOpticalData.Edges.Select(OpticalDataEdge.From).ToList().AsReadOnly(),
            Convert.ToUInt32(allOpticalData.Edges.Count),
            DateTime.UtcNow
        );
    }

    public OpticalDataConnection(
        IReadOnlyList<OpticalDataEdge> edges,
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