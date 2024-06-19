using System;

namespace Metabase.GraphQl.DataX;

public sealed class OpticalDataEdgeIgsdb
    : DataEdgeBase<OpticalDataIgsdb>
{
    public OpticalDataEdgeIgsdb(
        OpticalDataIgsdb node
    )
        : base(
            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(node.Id)),
            node
        )
    {
    }
}