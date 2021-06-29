namespace Metabase.GraphQl.DataX
{
    public abstract class DataEdgeBase<TData> {
      // public DataEdgeBase(
      //   string cursor,
      //   TData node
      // )
      // {
      //   Cursor = cursor;
      //   Node = node;
      // }

      public string Cursor { get; set; } = "";
      public TData Node { get; set; } = default!;
    }
}
