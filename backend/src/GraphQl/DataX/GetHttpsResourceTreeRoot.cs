using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class GetHttpsResourceTreeRoot : GetHttpsResourceTreeVertex {
      public string VertexId { get; set; }
      public GetHttpsResource Value { get; set; }
    }
}
