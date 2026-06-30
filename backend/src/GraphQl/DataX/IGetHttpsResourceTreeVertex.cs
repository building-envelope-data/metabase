using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX;

[InterfaceType("GetHttpsResourceTreeVertex")]
public interface IGetHttpsResourceTreeVertex
{
    [property: GraphQLType<NonNullType<IdType>>] string VertexId { get; }
    GetHttpsResource Value { get; }
}