using System;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Metabase.GraphQl
{
    public abstract class Edge<TNode, TNodeByIdDataLoader>
        where TNodeByIdDataLoader : IDataLoader<Guid, TNode?>
    {
        private readonly Guid _nodeId;

        protected Edge(
            Guid nodeId
            )
        {
            _nodeId = nodeId;
        }

        public Task<TNode> GetNode(
            [DataLoader] TNodeByIdDataLoader nodeById,
            CancellationToken cancellationToken
            )
        {
            return nodeById.LoadAsync(_nodeId, cancellationToken)!;
        }
    }
}