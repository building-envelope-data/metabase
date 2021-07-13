namespace Metabase.GraphQl.DataX
{
    public sealed class OpticalDataEdge
    : DataEdgeBase<OpticalData>
    {
        public OpticalDataEdge(
        string cursor,
        OpticalData node
        )
        : base(
            cursor,
            node
        )
        {
        }
    }
}
