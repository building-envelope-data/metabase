using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.CalorimetricDataX;

public sealed record CalorimetricDataConnection(
    IReadOnlyList<CalorimetricDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<CalorimetricDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);