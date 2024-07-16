namespace Metabase.GraphQl.DataX;

public sealed class GeometricDataEdge
    : DataEdgeBase<GeometricData>
{
    public GeometricDataEdge(
        string cursor,
        GeometricData node
    )
        : base(
            cursor,
            node
        )
    {
    }
}