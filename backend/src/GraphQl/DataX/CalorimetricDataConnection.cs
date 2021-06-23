using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class CalorimetricDataConnection {
      public List<CalorimetricDataEdge> Edges { get; set; }
      public List<CalorimetricData> Nodes { get; set; }
      public PageInfo PageInfo { get; set; }
      public DateTime Timestamp { get; set; }
    }
}
