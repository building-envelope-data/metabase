using System;
using System.Collections.Generic;
using System.Linq;

namespace Metabase.GraphQl.DataX;

public sealed class OpticalDataConnection
    : DataConnectionBase<OpticalDataEdge, OpticalData>
{
    internal static OpticalDataConnection? From(OpticalDataConnectionIgsdb? allOpticalData)
    {
        if (allOpticalData is null) {
            return null;
        }
        return new OpticalDataConnection(
            allOpticalData.Edges.Select(OpticalDataEdge.From).ToList().AsReadOnly(),
            allOpticalData.Nodes.Select(OpticalData.From).ToList().AsReadOnly(),
            allOpticalData.TotalCount,
            allOpticalData.Timestamp
        );
    }

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