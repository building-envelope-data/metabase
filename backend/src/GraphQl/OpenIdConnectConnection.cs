using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using GreenDonut;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl;

public abstract class OpenIdConnectConnection<TAssociation, TAssociationsByAssociateIdDataLoader, TEdge>(
    OpenIdConnectApplication subject,
    Func<TAssociation, TEdge> createEdge)
    where TAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
{
    private readonly Func<TAssociation, TEdge> _createEdge = createEdge;

    protected OpenIdConnectApplication Subject { get; } = subject;

    public async IAsyncEnumerable<TEdge> GetEdgesAsync(
        TAssociationsByAssociateIdDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        foreach (var association in await dataLoader.LoadAsync(Subject.Id, cancellationToken) ?? [])
        {
            yield return _createEdge(association);
        }
    }
}