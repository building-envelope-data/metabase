namespace Metabase.GraphQl.DataX;

public sealed record GetHttpsResourceTreeNonRootVertex(
    string VertexId,
    GetHttpsResource Value,
    string ParentId,
    ToTreeVertexAppliedConversionMethod AppliedConversionMethod
)
: IGetHttpsResourceTreeVertex;