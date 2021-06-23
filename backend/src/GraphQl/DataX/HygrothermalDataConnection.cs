using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class HygrothermalDataConnection {
      public List<HygrothermalDataEdge> Edges { get; set; }
      public List<HygrothermalData> Nodes { get; set; }
      public PageInfo PageInfo { get; set; }
      public DateTime Timestamp { get; set; }
    }
}
