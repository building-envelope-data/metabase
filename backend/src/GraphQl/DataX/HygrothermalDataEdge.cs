using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class HygrothermalDataEdge {
      public string Cursor { get; set; }
      public HygrothermalData Node { get; set; }
    }
}
