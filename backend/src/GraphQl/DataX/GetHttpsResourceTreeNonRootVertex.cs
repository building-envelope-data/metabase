namespace Metabase.GraphQl.DataX;

public sealed class GetHttpsResourceTreeNonRootVertex
    : IGetHttpsResourceTreeVertex
{
    public GetHttpsResourceTreeNonRootVertex(
        string vertexId,
        GetHttpsResource value,
        string parentId,
        ToTreeVertexAppliedConversionMethod appliedConversionMethod
    )
    {
        VertexId = vertexId;
        Value = value;
        ParentId = parentId;
        AppliedConversionMethod = appliedConversionMethod;
    }

    public string VertexId { get; }
    public string ParentId { get; }
    public ToTreeVertexAppliedConversionMethod AppliedConversionMethod { get; }
    public GetHttpsResource Value { get; }
}