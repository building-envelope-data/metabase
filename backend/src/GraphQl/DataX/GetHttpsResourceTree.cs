using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed record GetHttpsResourceTree(
    GetHttpsResourceTreeRoot Root,
    IReadOnlyList<GetHttpsResourceTreeNonRootVertex> NonRootVertices
);