using Infrastructure.Models;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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