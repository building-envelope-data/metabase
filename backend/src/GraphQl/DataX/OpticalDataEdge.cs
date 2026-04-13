using System;

namespace Metabase.GraphQl.DataX;

public sealed record OpticalDataEdge(
    string Cursor,
    OpticalData Node
) : DataEdgeBase<OpticalData>(
    Cursor,
    Node
);