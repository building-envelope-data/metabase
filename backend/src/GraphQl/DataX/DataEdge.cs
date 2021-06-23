using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class DataEdge {
      public string Cursor { get; set; }
      public IData Node { get; set; }
    }
}
