using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using HotChocolate.Types.Pagination;
using Metabase.Data;
using Metabase.GraphQl.Extensions;

namespace Metabase.GraphQl;

public abstract class PaginatedConnection<
    TSubject,
    TAssociation,
    TEdge,
    TAssociationsByOneIdDataLoader
>(
    TSubject subject,
    Func<TAssociation, string, TEdge> createEdge,
    PagingArguments pagingArguments,
    QueryContext<TAssociation> queryContext
)
    where TSubject : IEntity
    where TAssociation : class
    where TAssociationsByOneIdDataLoader : IDataLoader<Guid, Page<TAssociation>>
{
    protected TSubject Subject { get; } = subject;

    [Cost(0)]
    public ValueTask<int> GetTotalCountAsync(
        TAssociationsByOneIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return dataLoader
            .With(pagingArguments, queryContext)
            .LoadAsync(Subject.Id, cancellationToken)
            .GetTotalCountAsync();
    }

    [Cost(0)]
    public ValueTask<ConnectionPageInfo> GetPageInfoAsync(
        TAssociationsByOneIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return dataLoader
            .With(pagingArguments, queryContext)
            .LoadAsync(Subject.Id, cancellationToken)
            .GetPageInfoAsync();
    }

    [Cost(0)]
    public async IAsyncEnumerable<TEdge> GetEdgesAsync(
        TAssociationsByOneIdDataLoader dataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        var page =
            await dataLoader
            .With(pagingArguments, queryContext)
            .LoadAsync(Subject.Id, cancellationToken);
        if (page is null)
        {
            yield break;
        }
        foreach (var entry in page.Entries)
        {
            yield return createEdge(entry.Item, page.CreateCursor(entry));
        }
    }
}