namespace Metabase.GraphQl.DataX;

public sealed record CalorimetricDataEdge(
    string Cursor,
    CalorimetricData Node
) : DataEdgeBase<CalorimetricData>(
    Cursor,
    Node
);