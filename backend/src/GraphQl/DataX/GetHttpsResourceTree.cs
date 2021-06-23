using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class GetHttpsResourceTree {
      public GetHttpsResourceTreeRoot Root { get; set; }
      public List<GetHttpsResourceTreeNonRootVertex> NonRootVertices { get; set; }
    }
}
