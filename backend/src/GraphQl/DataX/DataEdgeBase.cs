using HotChocolate.CostAnalysis.Types;

namespace Metabase.GraphQl.DataX;

public abstract record DataEdgeBase<TData>(
    [property: Cost(0)] string Cursor,
    [property: Cost(0)] TData Node
);