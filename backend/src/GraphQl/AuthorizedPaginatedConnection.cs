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

public abstract class AuthorizedPaginatedConnection<TSubject, TAssociation, TEdge, TAssociationsByOneIdDataLoader, TAuthorization>(
    TSubject subject,
    Func<TAssociation, string, TEdge> createEdge,
    Func<ClaimsPrincipal, TAuthorization, CancellationToken, Task<bool>> isAuthorized,
    PagingArguments pagingArguments,
    QueryContext<TAssociation> queryContext
) : PaginatedConnection<TSubject, TAssociation, TEdge, TAssociationsByOneIdDataLoader>(
    subject,
    createEdge,
    pagingArguments,
    queryContext
)
    where TSubject : IEntity
    where TAssociation : class
    where TAssociationsByOneIdDataLoader : IDataLoader<Guid, Page<TAssociation>>
{
    [Cost(0)]
    public async IAsyncEnumerable<TEdge> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        TAuthorization authorization,
        TAssociationsByOneIdDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await isAuthorized(claimsPrincipal, authorization, cancellationToken))
        {
            yield break;
        }
        await foreach (var edge in base.GetEdgesAsync(dataLoader, cancellationToken))
        {
            yield return edge;
        }
    }
}