using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class GetHttpsResourceTreeNonRootVertex : GetHttpsResourceTreeVertex {
      public string VertexId { get; set; }
      public GetHttpsResource Value { get; set; }
      public string ParentId { get; set; }
      public ToTreeVertexAppliedConversionMethod AppliedConversionMethod { get; set; }
    }
}
