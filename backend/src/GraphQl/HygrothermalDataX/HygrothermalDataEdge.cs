using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.HygrothermalDataX;

public sealed record HygrothermalDataEdge(
    string Cursor,
    HygrothermalData Node
) : DataEdge<HygrothermalData>(
    Cursor,
    Node
);