using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class PageInfo {
      public bool HasNextPage { get; set; }
      public bool HasPreviousPage { get; set; }
      public string StartCursor { get; set; }
      public string EndCursor { get; set; }
      public uint Count { get; set; }
    }
}
