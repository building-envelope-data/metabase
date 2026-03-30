using System;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.CostAnalysis.Types;

namespace Metabase.GraphQl;

public abstract record PaginatedEdge<TNode>(
    TNode Node,
    string Cursor
);

public abstract class PaginatedEdge<TNode, TNodeByIdDataLoader>(
    Guid nodeId,
    string cursor
)
    where TNodeByIdDataLoader : IDataLoader<Guid, TNode>
    where TNode : notnull
{
    [Cost(0)]
    public Task<TNode> GetNodeAsync(
        TNodeByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        return byId.LoadRequiredAsync(nodeId, cancellationToken);
    }

    public string Cursor => cursor;
}