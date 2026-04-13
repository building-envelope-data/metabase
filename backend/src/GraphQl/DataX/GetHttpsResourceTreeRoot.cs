using System;

namespace Metabase.GraphQl.DataX;

public sealed record GetHttpsResourceTreeRoot(
    string VertexId,
    GetHttpsResource Value
) : IGetHttpsResourceTreeVertex;