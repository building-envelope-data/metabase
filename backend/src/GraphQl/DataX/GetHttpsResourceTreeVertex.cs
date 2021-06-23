using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public interface GetHttpsResourceTreeVertex {
      string VertexId { get; set; }
      GetHttpsResource Value { get; set; }
    }
}
