using System;
using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.PhotovoltaicDataX;

public sealed record PhotovoltaicDataConnection(
    IReadOnlyList<PhotovoltaicDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnectionBase<PhotovoltaicDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);