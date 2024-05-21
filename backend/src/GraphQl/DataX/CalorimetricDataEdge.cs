namespace Metabase.GraphQl.DataX;

public sealed class CalorimetricDataEdge
    : DataEdgeBase<CalorimetricData>
{
    public CalorimetricDataEdge(
        string cursor,
        CalorimetricData node
    )
        : base(
            cursor,
            node
        )
    {
    }
}