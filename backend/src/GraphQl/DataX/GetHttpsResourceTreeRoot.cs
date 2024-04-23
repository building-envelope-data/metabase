namespace Metabase.GraphQl.DataX
{
    public sealed class GetHttpsResourceTreeRoot : IGetHttpsResourceTreeVertex
    {
        public GetHttpsResourceTreeRoot(
            GetHttpsResource value
        )
        {
            Value = value;
        }

        // public string VertexId { get; }
        public GetHttpsResource Value { get; }
    }
}