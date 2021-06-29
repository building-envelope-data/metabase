using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class GetHttpsResourceTreeRoot : IGetHttpsResourceTreeVertex {
      // public string VertexId { get; set; }
      public GetHttpsResource Value { get; set; }
    }
}
