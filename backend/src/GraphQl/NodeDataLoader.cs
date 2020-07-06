using Icon.Infrastructure.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public class NodeDataLoader
      : ModelDataLoader<Node, IModel>
    {
        public NodeDataLoader(IQueryBus queryBus)
          : base(NodeBase.FromModel, queryBus)
        {
        }
    }
}