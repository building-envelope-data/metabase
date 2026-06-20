using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.OpticalDataX;

public sealed record OpticalDataConnection(
    IReadOnlyList<OpticalDataEdge> Edges,
    int TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnection<OpticalDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);