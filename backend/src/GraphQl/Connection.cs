using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Data;

namespace Metabase.GraphQl;

public abstract class Connection<TSubject, TAssociation, TEdge, TAssociationsByOneIdDataLoader>(
    TSubject subject,
    Func<TAssociation, TEdge> createEdge,
    QueryContext<TAssociation> queryContext
)
    where TSubject : IEntity
    where TAssociationsByOneIdDataLoader : IDataLoader<Guid, TAssociation[]>
{
    protected TSubject Subject { get; } = subject;

    [Cost(0)]
    public async Task<uint> GetTotalCountAsync(
        TAssociationsByOneIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return (uint)(
            (
                await dataLoader
                .With(queryContext)
                .LoadAsync(Subject.Id, cancellationToken)
            )
            ?.Length ?? 0
        );
    }

    [Cost(0)]
    public async IAsyncEnumerable<TEdge> GetEdgesAsync(
        TAssociationsByOneIdDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        foreach (var association in await dataLoader.With(queryContext).LoadAsync(Subject.Id, cancellationToken) ?? [])
        {
            yield return createEdge(association);
        }
    }
}