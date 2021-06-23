using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class OpticalDataEdge {
      public string Cursor { get; set; }
      public OpticalData Node { get; set; }
    }
}
