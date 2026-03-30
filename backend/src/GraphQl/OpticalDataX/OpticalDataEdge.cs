using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.OpticalDataX;

public sealed record OpticalDataEdge(
    string Cursor,
    OpticalData Node
) : DataEdgeBase<OpticalData>(
    Cursor,
    Node
);