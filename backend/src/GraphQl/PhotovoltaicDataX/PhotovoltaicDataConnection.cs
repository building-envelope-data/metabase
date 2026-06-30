using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.PhotovoltaicDataX;

public sealed record PhotovoltaicDataConnection(
    IReadOnlyList<PhotovoltaicDataEdge>? Edges,
    int TotalCount,
    ConnectionPageInfo PageInfo
) : DataConnection<PhotovoltaicDataEdge>(
    Edges,
    TotalCount,
    PageInfo
);