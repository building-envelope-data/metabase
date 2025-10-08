using System;
using System.Collections.Generic;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.DataX;

public sealed record PhotovoltaicDataConnection(
    IReadOnlyList<PhotovoltaicDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<PhotovoltaicDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);