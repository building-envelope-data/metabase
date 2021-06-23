using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class PhotovoltaicDataConnection {
      public List<PhotovoltaicDataEdge> Edges { get; set; }
      public List<PhotovoltaicData> Nodes { get; set; }
      public PageInfo PageInfo { get; set; }
      public DateTime Timestamp { get; set; }
    }
}
