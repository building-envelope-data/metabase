using System;

namespace Metabase.GraphQl.DataX;

public sealed record GeometricDataEdge(
    string Cursor,
    GeometricData Node
) : DataEdgeBase<GeometricData>(
    Cursor,
    Node
);