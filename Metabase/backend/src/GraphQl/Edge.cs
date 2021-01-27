using System;

namespace Metabase.GraphQl
{
    public abstract class Edge<TNode>
    {
        protected readonly Guid NodeId;

        protected Edge(
            Guid nodeId
            )
        {
            NodeId = nodeId;
        }
    }
}