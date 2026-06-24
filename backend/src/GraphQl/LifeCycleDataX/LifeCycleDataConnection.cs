using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.LifeCycleDataX;

public sealed record LifeCycleDataConnection(
    IReadOnlyList<LifeCycleDataEdge>? Edges,
    int TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnection<LifeCycleDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);