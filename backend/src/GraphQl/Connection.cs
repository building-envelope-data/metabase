using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;

namespace Metabase.GraphQl;

public abstract class Connection<TSubject, TAssociation, TAssociationsByAssociateIdDataLoader, TEdge>(
    TSubject subject,
    Func<TAssociation, TEdge> createEdge,
    QueryContext<TAssociation> queryContext
    )
    where TSubject : IEntity
    where TAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
{
    protected TSubject Subject { get; } = subject;

    public async Task<uint> GetTotalCountAsync(
        TAssociationsByAssociateIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return (uint)(await dataLoader.With(queryContext).LoadRequiredAsync(Subject.Id, cancellationToken)).Length;
    }

    public async IAsyncEnumerable<TEdge> GetEdgesAsync(
        TAssociationsByAssociateIdDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        foreach (var association in await dataLoader.With(queryContext).LoadRequiredAsync(Subject.Id, cancellationToken))
        {
            yield return createEdge(association);
        }
    }
}