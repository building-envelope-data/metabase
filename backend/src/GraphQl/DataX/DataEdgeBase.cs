namespace Metabase.GraphQl.DataX;

public abstract class DataEdgeBase<TData>
{
    protected DataEdgeBase(
        string cursor,
        TData node
    )
    {
        Cursor = cursor;
        Node = node;
    }

    public string Cursor { get; }
    public TData Node { get; }
}