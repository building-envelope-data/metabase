using System;
using System.Collections.Generic;
using System.Linq;

namespace Metabase.GraphQl.DataX;

public sealed class DataConnection
    : DataConnectionBase<DataEdge, IData>
{
    internal static DataConnection? From(DataConnectionIgsdb? connection)
    {
        if (connection is null) {
            return null;
        }
        return new DataConnection(
            connection.Edges.Select(DataEdge.From).ToList().AsReadOnly(),
            connection.Nodes.Select(d => OpticalData.From((OpticalDataIgsdb) d)).ToList().AsReadOnly(),
            connection.TotalCount,
            connection.Timestamp
        );
    }

    public DataConnection(
        IReadOnlyList<DataEdge> edges,
        IReadOnlyList<IData> nodes,
        uint totalCount,
        DateTime timestamp
    )
        : base(
            edges,
            nodes,
            totalCount,
            timestamp
        )
    {
    }
}