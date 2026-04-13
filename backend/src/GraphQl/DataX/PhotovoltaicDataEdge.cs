namespace Metabase.GraphQl.DataX;

public sealed record PhotovoltaicDataEdge(
    string Cursor,
    PhotovoltaicData Node
) : DataEdgeBase<PhotovoltaicData>(
    Cursor,
    Node
);