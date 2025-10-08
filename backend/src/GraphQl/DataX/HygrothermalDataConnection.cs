using System;
using System.Collections.Generic;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.DataX;

public sealed record HygrothermalDataConnection(
    IReadOnlyList<HygrothermalDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<HygrothermalDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);