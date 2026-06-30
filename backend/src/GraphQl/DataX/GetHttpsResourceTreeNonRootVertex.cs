using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX;

public sealed record GetHttpsResourceTreeNonRootVertex(
    [property: GraphQLType<NonNullType<IdType>>] string VertexId,
    GetHttpsResource Value,
    [property: GraphQLType<NonNullType<IdType>>] string ParentId,
    ToTreeVertexAppliedConversionMethod AppliedConversionMethod
)
: IGetHttpsResourceTreeVertex;