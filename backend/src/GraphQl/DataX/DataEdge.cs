using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class DataEdge {
      public string Cursor { get; set; }
      public DataX Node { get; set; }
    }
}
