using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX;

public sealed record GetHttpsResourceTreeRoot(
    [property: GraphQLType<NonNullType<IdType>>] string VertexId,
    GetHttpsResource Value
) : IGetHttpsResourceTreeVertex;