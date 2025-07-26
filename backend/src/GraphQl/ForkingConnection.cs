using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using GreenDonut;
using Metabase.Data;

namespace Metabase.GraphQl;

public abstract class ForkingConnection<TSubject, TAssociation, TSomeAssociationsByAssociateIdDataLoader,
    TOtherAssociationsByAssociateIdDataLoader, TEdge>(
    TSubject subject,
    bool useFirstDataLoader,
    Func<TAssociation, TEdge> createEdge
    )
    where TSubject : IEntity
    where TSomeAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
    where TOtherAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
{
    private readonly Func<TAssociation, TEdge> _createEdge = createEdge;
    private readonly bool _useFirstDataLoader = useFirstDataLoader;

    protected TSubject Subject { get; } = subject;

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

    private async IAsyncEnumerable<TEdge> GetEdgesAsync<TDataLoader>(
        TDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
        where TDataLoader : IDataLoader<Guid, TAssociation[]>
    {
        foreach (var association in await dataLoader.LoadAsync(Subject.Id, cancellationToken) ?? [])
        {
            yield return _createEdge(association);
        }
    }
}