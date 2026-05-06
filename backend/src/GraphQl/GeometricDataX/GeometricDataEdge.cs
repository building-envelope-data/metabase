using System;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.GeometricDataX;

public sealed record GeometricDataEdge(
    string Cursor,
    GeometricData Node
) : DataEdgeBase<GeometricData>(
    Cursor,
    Node
);