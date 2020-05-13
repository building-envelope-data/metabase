using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class NodeDataLoader
      : ModelDataLoader<Node, Models.IModel>
    {
        public NodeDataLoader(IQueryBus queryBus)
          : base(NodeBase.FromModel, queryBus)
        {
        }
    }
}