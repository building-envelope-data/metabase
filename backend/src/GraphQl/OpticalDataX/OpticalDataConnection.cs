using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.OpticalDataX;

public sealed record OpticalDataConnection(
    IReadOnlyList<OpticalDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<OpticalDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);