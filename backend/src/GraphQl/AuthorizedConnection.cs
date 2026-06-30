using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Data;

namespace Metabase.GraphQl;

public abstract class AuthorizedConnection<TSubject, TAssociation, TEdge, TAssociationsByOneIdDataLoader, TAuthorization>(
    TSubject subject,
    Func<TAssociation, TEdge> createEdge,
    Func<ClaimsPrincipal, TSubject, TAuthorization, CancellationToken, Task<bool>> isAuthorized,
    QueryContext<TAssociation> queryContext
) : Connection<TSubject, TAssociation, TEdge, TAssociationsByOneIdDataLoader>(subject, createEdge, queryContext)
    where TSubject : IEntity
    where TAssociationsByOneIdDataLoader : IDataLoader<Guid, TAssociation[]>
{
    [Cost(0)]
    public async IAsyncEnumerable<TEdge> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        TAuthorization authorization,
        TAssociationsByOneIdDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await isAuthorized(claimsPrincipal, Subject, authorization, cancellationToken))
        {
            yield break;
        }
        await foreach (var edge in GetEdgesAsync(dataLoader, cancellationToken))
        {
            yield return edge;
        }
    }
}