namespace Metabase.GraphQl.DataX;

public sealed class DataEdge
    : DataEdgeBase<IData>
{
    public DataEdge(
        string cursor,
        IData node
    )
        : base(
            cursor,
            node
        )
    {
    }
}