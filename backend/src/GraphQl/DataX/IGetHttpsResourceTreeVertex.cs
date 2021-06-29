using HotChocolate.Types;

namespace Metabase.GraphQl.DataX
{
    [InterfaceType("GetHttpsResourceTreeVertex")]
    public interface IGetHttpsResourceTreeVertex
    {
      // string VertexId { get; set; }
      GetHttpsResource Value { get; set; }
    }
}
