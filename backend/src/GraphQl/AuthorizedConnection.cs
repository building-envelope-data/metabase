using System;
using System.Collections.Generic;
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

    public async Task<IEnumerable<TEdge>> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        TAuthorization authorization,
        TAssociationsByAssociateIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        if (!await _isAuthorized(claimsPrincipal, Subject, authorization, cancellationToken))
        {
            return [];
        }
        return await GetEdgesAsync(dataLoader, cancellationToken);
    }
}