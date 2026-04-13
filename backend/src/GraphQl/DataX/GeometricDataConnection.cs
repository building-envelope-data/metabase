using System.Collections.Generic;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.DataX;

public sealed record GeometricDataConnection(
    IReadOnlyList<GeometricDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<GeometricDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);