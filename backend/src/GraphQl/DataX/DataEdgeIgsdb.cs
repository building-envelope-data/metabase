namespace Metabase.GraphQl.DataX;

public sealed class DataEdgeIgsdb
    : DataEdgeBase<IDataIgsdb>
{
    public DataEdgeIgsdb(
        string cursor,
        IDataIgsdb node
    )
        : base(
            cursor,
            node
        )
    {
    }
}