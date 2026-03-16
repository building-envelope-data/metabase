namespace Metabase.GraphQl.DataX;

public sealed record LifeCycleDataEdge(
    string Cursor,
    LifeCycleData Node
) : DataEdgeBase<LifeCycleData>(
    Cursor,
    Node
);