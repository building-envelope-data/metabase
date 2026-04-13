using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.DataX;

public sealed record DataConnection(
    IReadOnlyList<DataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<DataEdge>(
    Edges,
    TotalCount,
    PageInfo
);