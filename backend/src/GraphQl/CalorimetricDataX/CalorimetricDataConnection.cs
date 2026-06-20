using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.CalorimetricDataX;

public sealed record CalorimetricDataConnection(
    IReadOnlyList<CalorimetricDataEdge> Edges,
    int TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnection<CalorimetricDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);