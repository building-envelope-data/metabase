using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.PhotovoltaicDataX;

public sealed record PhotovoltaicDataEdge(
    string Cursor,
    PhotovoltaicData Node
) : DataEdgeBase<PhotovoltaicData>(
    Cursor,
    Node
);