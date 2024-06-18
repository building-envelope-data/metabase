using System;

namespace Metabase.GraphQl.DataX;

public sealed class OpticalDataEdge
    : DataEdgeBase<OpticalData>
{
    internal static OpticalDataEdge From(OpticalDataEdgeIgsdb edge)
    {
        return new OpticalDataEdge(
            edge.Cursor,
            OpticalData.From(edge.Node)
        );
    }

    public OpticalDataEdge(
        string cursor,
        OpticalData node
    )
        : base(
            cursor,
            node
        )
    {
    }
}