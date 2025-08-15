using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;

namespace Metabase.GraphQl;

public abstract class ForkingConnection<TSubject, TAssociation, TSomeAssociationsByAssociateIdDataLoader,
    TOtherAssociationsByAssociateIdDataLoader, TEdge>(
    TSubject subject,
    bool useFirstDataLoader,
    Func<TAssociation, TEdge> createEdge,
    QueryContext<TAssociation>? queryContext
    )
    where TSubject : IEntity
    where TSomeAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
    where TOtherAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
{
    private readonly Func<TAssociation, TEdge> _createEdge = createEdge;
    private readonly QueryContext<TAssociation>? _queryContext = queryContext;
    private readonly bool _useFirstDataLoader = useFirstDataLoader;

    protected TSubject Subject { get; } = subject;

    public Task<uint> GetTotalCountAsync(
        TSomeAssociationsByAssociateIdDataLoader someDataLoader,
        TOtherAssociationsByAssociateIdDataLoader otherDataLoader,
        CancellationToken cancellationToken
    )
    {
        return _useFirstDataLoader
            ? GetTotalCountAsync(someDataLoader, cancellationToken)
            : GetTotalCountAsync(otherDataLoader, cancellationToken);
    }

    public IAsyncEnumerable<TEdge> GetEdgesAsync(
        TSomeAssociationsByAssociateIdDataLoader someDataLoader,
        TOtherAssociationsByAssociateIdDataLoader otherDataLoader,
        CancellationToken cancellationToken
    )
    {
        return _useFirstDataLoader
            ? GetEdgesAsync(someDataLoader, cancellationToken)
            : GetEdgesAsync(otherDataLoader, cancellationToken);
    }

    private async Task<uint> GetTotalCountAsync<TDataLoader>(
        TDataLoader dataLoader,
        CancellationToken cancellationToken
    )
        where TDataLoader : IDataLoader<Guid, TAssociation[]>
    {
        return (uint)(await dataLoader.With(_queryContext).LoadRequiredAsync(Subject.Id, cancellationToken)).Length;
    }

    private async IAsyncEnumerable<TEdge> GetEdgesAsync<TDataLoader>(
        TDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
        where TDataLoader : IDataLoader<Guid, TAssociation[]>
    {
        foreach (var association in await dataLoader.With(_queryContext).LoadRequiredAsync(Subject.Id, cancellationToken))
        {
            yield return _createEdge(association);
        }
    }
}