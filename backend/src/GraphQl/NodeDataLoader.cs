using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class NodeForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<Node, Models.IModel>
    {
        public NodeForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(NodeBase.FromModel, queryBus)
        {
        }
    }
}