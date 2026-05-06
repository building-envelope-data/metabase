using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.GeometricDataX;

public sealed record GeometricDataConnection(
    IReadOnlyList<GeometricDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<GeometricDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);