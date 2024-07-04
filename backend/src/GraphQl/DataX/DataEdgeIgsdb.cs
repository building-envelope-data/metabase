using System;

namespace Metabase.GraphQl.DataX;

public sealed class DataEdgeIgsdb
    : DataEdgeBase<IDataIgsdb>
{
    public DataEdgeIgsdb(
        IDataIgsdb node
    )
        : base(
            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(node.Id)),
            node
        )
    {
    }
}