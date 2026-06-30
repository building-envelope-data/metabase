using System;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.CostAnalysis.Types;

namespace Metabase.GraphQl;

public abstract class Edge<TNode, TNodeByIdDataLoader>(
    Guid nodeId
)
    where TNodeByIdDataLoader : IDataLoader<Guid, TNode>
{
    [Cost(0)]
    public async Task<TNode> GetNodeAsync(
        TNodeByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        return (await byId.LoadAsync(nodeId, cancellationToken))!;
    }
}