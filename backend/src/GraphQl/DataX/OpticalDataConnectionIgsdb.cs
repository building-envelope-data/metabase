using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class OpticalDataConnectionIgsdb
{
    public IReadOnlyList<OpticalDataEdgeIgsdb> Edges { get; }

    public OpticalDataConnectionIgsdb(
        IReadOnlyList<OpticalDataEdgeIgsdb> edges
    )
    {
        Edges = edges;
    }
}