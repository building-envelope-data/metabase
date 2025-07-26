using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using Metabase.Data;

namespace Metabase.GraphQl;

public abstract class AuthorizedConnection<TSubject, TAssociation, TAssociationsByAssociateIdDataLoader, TEdge, TAuthorization>(
    TSubject subject,
    Func<TAssociation, TEdge> createEdge,
    Func<ClaimsPrincipal, TSubject, TAuthorization, CancellationToken, Task<bool>> isAuthorized
) : Connection<TSubject, TAssociation, TAssociationsByAssociateIdDataLoader, TEdge>(subject, createEdge)
    where TSubject : IEntity
    where TAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
{
    private readonly Func<ClaimsPrincipal, TSubject, TAuthorization, CancellationToken, Task<bool>> _isAuthorized = isAuthorized;

    public async IAsyncEnumerable<TEdge> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        TAuthorization authorization,
        TAssociationsByAssociateIdDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await _isAuthorized(claimsPrincipal, Subject, authorization, cancellationToken))
        {
            yield break;
        }
        await foreach (var edge in GetEdgesAsync(dataLoader, cancellationToken))
        {
            yield return edge;
        }
    }
}