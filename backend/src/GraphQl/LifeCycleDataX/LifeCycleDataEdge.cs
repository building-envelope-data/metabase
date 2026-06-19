using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.LifeCycleDataX;

public sealed record LifeCycleDataEdge(
    string Cursor,
    LifeCycleData Node
) : DataEdge<LifeCycleData>(
    Cursor,
    Node
);