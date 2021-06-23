using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class DataConnection {
      public List<DataEdge> Edges { get; set; }
      public List<IData> Nodes { get; set; }
      public /*TODO add `u`*/int TotalCount { get; set; }
      // public PageInfo PageInfo { get; set; } // TODO Resolve clash with `PageInfo` provided by `HotChocolate` 
      public DateTime Timestamp { get; set; }
    }
}
