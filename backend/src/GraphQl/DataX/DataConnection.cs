using System;
using System.Collections.Generic;
using System.Linq;

namespace Metabase.GraphQl.DataX;

public sealed class DataConnection
    : DataConnectionBase<DataEdge>
{
    internal static DataConnection? From(DataConnectionIgsdb? connection)
    {
        if (connection is null) {
            return null;
        }
        return new DataConnection(
            connection.Edges.Select(DataEdge.From).ToList().AsReadOnly(),
            connection.TotalCount,
            connection.Timestamp
        );
    }

    public DataConnection(
        IReadOnlyList<DataEdge> edges,
        uint totalCount,
        DateTime timestamp
    )
        : base(
            edges,
            totalCount,
            timestamp
        )
    {
    }
}