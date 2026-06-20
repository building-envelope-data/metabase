using System.Collections.Generic;
using HotChocolate.CostAnalysis.Types;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.DataX;

public abstract record DataConnection<TDataEdge>(
    [property: Cost(0)] IReadOnlyList<TDataEdge> Edges,
    [property: Cost(0)] int TotalCount,
    [property: Cost(0)] ConnectionPageInfo PageInfo
);