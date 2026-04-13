using System;
using System.Collections.Generic;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.DataX;

public sealed record CalorimetricDataConnection(
    IReadOnlyList<CalorimetricDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<CalorimetricDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);