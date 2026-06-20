using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.GeometricDataX;

public sealed record GeometricDataConnection(
    IReadOnlyList<GeometricDataEdge> Edges,
    int TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnection<GeometricDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);