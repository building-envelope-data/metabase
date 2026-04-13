using System.Collections.Generic;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.DataX;

public sealed record LifeCycleDataConnection(
    IReadOnlyList<LifeCycleDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<LifeCycleDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);