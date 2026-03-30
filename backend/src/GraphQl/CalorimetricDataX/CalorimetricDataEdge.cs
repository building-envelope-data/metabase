using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.CalorimetricDataX;

public sealed record CalorimetricDataEdge(
    string Cursor,
    CalorimetricData Node
) : DataEdgeBase<CalorimetricData>(
    Cursor,
    Node
);