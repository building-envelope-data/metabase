using System.Collections.Generic;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.DataX;

public abstract record DataConnectionBase<TDataEdge>(
    IReadOnlyList<TDataEdge> Edges,
    uint TotalCount,
    ConnectionPageInfo PageInfo
);