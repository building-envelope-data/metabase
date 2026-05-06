using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.LifeCycleDataX;

public sealed record LifeCycleDataConnection(
    IReadOnlyList<LifeCycleDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<LifeCycleDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);