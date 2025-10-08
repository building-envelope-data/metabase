namespace Metabase.GraphQl.DataX;

public sealed record HygrothermalDataEdge(
    string Cursor,
    HygrothermalData Node
) : DataEdgeBase<HygrothermalData>(
    Cursor,
    Node
);