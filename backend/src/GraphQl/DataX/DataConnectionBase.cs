using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public abstract class DataConnectionBase<TDataEdge, TData>
    {
      // public DataConnectionBase(
      //   List<TDataEdge> edges,
      //   List<TData> nodes,
      //   /*TODO add `u`*/int totalCount,
      //   DateTime timestamp
      // )
      // {
      //   Edges = edges;
      //   Nodes = nodes;
      //   TotalCount = totalCount;
      //   Timestamp = timestamp;
      // }

      public List<TDataEdge> Edges { get; set; } = new List<TDataEdge>();
      public List<TData> Nodes { get; set; } = new List<TData>();
      public /*TODO add `u`*/int TotalCount { get; }
      // public PageInfo PageInfo { get; set; } // TODO Resolve clash with `PageInfo` provided by `HotChocolate` 
      public DateTime Timestamp { get; }
    }
}
