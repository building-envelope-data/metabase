namespace Metabase.GraphQl.DataX;

public sealed class DataEdge
    : DataEdgeBase<IData>
{
    internal static DataEdge From(DataEdgeIgsdb edge)
    {
        return new DataEdge(
            edge.Cursor,
            OpticalData.From((OpticalDataIgsdb) edge.Node)
        );
    }

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