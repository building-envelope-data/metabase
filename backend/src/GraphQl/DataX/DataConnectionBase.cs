using System;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX
{
    public abstract class DataConnectionBase<TDataEdge, TData>
    {
        protected DataConnectionBase(
          IReadOnlyList<TDataEdge> edges,
          IReadOnlyList<TData> nodes,
          uint totalCount,
          DateTime timestamp
        )
        {
            Edges = edges;
            Nodes = nodes;
            TotalCount = totalCount;
            Timestamp = timestamp;
        }

        public IReadOnlyList<TDataEdge> Edges { get; }

        public IReadOnlyList<TData> Nodes { get; }

        [GraphQLType(typeof(NonNegativeIntType))]
        public uint TotalCount { get; }

        // public PageInfo PageInfo { get; } // TODO Resolve clash with `PageInfo` provided by `HotChocolate` 

        public DateTime Timestamp { get; }
    }
}
