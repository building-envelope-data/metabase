using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class OpticalDataConnection {
      public List<OpticalDataEdge> Edges { get; set; }
      public List<OpticalData> Nodes { get; set; }
      public PageInfo PageInfo { get; set; }
      public DateTime Timestamp { get; set; }
    }
}
