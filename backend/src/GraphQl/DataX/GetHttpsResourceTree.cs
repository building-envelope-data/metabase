using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class GetHttpsResourceTree {
      public GetHttpsResourceTreeRoot Root { get; set; }
      public List<GetHttpsResourceTreeNonRootVertex> NonRootVertices { get; set; }
    }
}
