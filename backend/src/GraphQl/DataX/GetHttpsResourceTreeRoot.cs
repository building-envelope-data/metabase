using System;

namespace Metabase.GraphQl.DataX;

public sealed class GetHttpsResourceTreeRoot : IGetHttpsResourceTreeVertex
{
    internal static GetHttpsResourceTreeRoot From(GetHttpsResourceTreeRootIgsdb root)
    {
        return new GetHttpsResourceTreeRoot(
            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("root")),
            GetHttpsResource.From(root.Value)
        );
    }

    public GetHttpsResourceTreeRoot(
        string vertexId,
        GetHttpsResource value
    )
    {
        VertexId = vertexId;
        Value = value;
    }

    public string VertexId { get; }
    public GetHttpsResource Value { get; }
}