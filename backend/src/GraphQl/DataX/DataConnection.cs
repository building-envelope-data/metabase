using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class DataConnection {
      public List<DataEdge> Edges { get; set; }
      public List<DataX> Nodes { get; set; }
      public uint TotalCount { get; set; }
      public PageInfo PageInfo { get; set; }
      public DateTime Timestamp { get; set; }
    }
}
