using System;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX;

public abstract class DataConnectionBase<TDataEdge>
{
    protected DataConnectionBase(
        IReadOnlyList<TDataEdge> edges,
        uint totalCount,
        DateTime timestamp
    )
    {
        Edges = edges;
        TotalCount = totalCount;
        Timestamp = timestamp;
    }

    public IReadOnlyList<TDataEdge> Edges { get; }

    [GraphQLType<NonNegativeIntType>] public uint TotalCount { get; }

    // public PageInfo PageInfo { get; } // TODO Resolve clash with `PageInfo` provided by `HotChocolate` 

    public DateTime Timestamp { get; }
}