using System;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.GeometricDataX;

public sealed record GeometricDataEdge(
    string Cursor,
    GeometricData Node
) : DataEdge<GeometricData>(
    Cursor,
    Node
);