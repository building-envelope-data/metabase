using Infrastructure.GraphQl;
using Infrastructure.Models;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public class NodeDataLoader
      : ModelDataLoader<INode, IModel>
    {
        public NodeDataLoader(IQueryBus queryBus)
          : base(Node.FromModel, queryBus)
        {
        }
    }
}