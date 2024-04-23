using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class GetHttpsResourceTree
{
    public GetHttpsResourceTree(
        GetHttpsResourceTreeRoot root
    )
    {
        Root = root;
    }

    public GetHttpsResourceTreeRoot Root { get; }
    // public IReadOnlyList<GetHttpsResourceTreeNonRootVertex> NonRootVertices { get; }
}