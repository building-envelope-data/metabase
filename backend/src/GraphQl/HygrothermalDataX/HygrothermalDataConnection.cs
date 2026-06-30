using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.HygrothermalDataX;

public sealed record HygrothermalDataConnection(
    IReadOnlyList<HygrothermalDataEdge>? Edges,
    int TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnection<HygrothermalDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);