using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class GetHttpsResourceTree
{
    internal static GetHttpsResourceTree From(GetHttpsResourceTreeIgsdb resourceTree)
    {
        return new GetHttpsResourceTree(
            GetHttpsResourceTreeRoot.From(resourceTree.Root),
            Array.Empty<GetHttpsResourceTreeNonRootVertex>().AsReadOnly()
        );
    }

    public GetHttpsResourceTree(
        GetHttpsResourceTreeRoot root,
        IReadOnlyList<GetHttpsResourceTreeNonRootVertex> nonRootVertices
    )
    {
        Root = root;
        NonRootVertices = nonRootVertices;
    }

    public GetHttpsResourceTreeRoot Root { get; }
    public IReadOnlyList<GetHttpsResourceTreeNonRootVertex> NonRootVertices { get; }
}