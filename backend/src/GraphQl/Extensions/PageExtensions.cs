using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.Extensions;

// Inspired by https://github.com/ChilliCream/graphql-platform/blob/main/src/HotChocolate/Data/src/Data/Extensions/HotChocolatePaginationResultExtensions.cs
public static class PageExtensions
{
    [Pure]
    public static async ValueTask<int> GetTotalCountAsync<TSource>(
        this Task<Page<TSource>?> pagePromise
    )
    {
        return (await pagePromise)?.TotalCount ?? 0;
    }

    [Pure]
    public static async ValueTask<ConnectionPageInfo> GetPageInfoAsync<TSource>(
        this Task<Page<TSource>?> pagePromise
    )
    {
        var page = await pagePromise;
        return new ConnectionPageInfo(
            page?.HasNextPage ?? false,
            page?.HasPreviousPage ?? false,
            page?.CreateStartCursor(),
            page?.CreateEndCursor()
        );
    }

    // [Pure]
    // public static async Task<Connection<TTarget>> ToConnectionAsync<TSource, TTarget>(
    //     this Task<Page<TSource>> pagePromise,
    //     Func<Page<TSource>, PageEntry<TSource>, Task<IEdge<TTarget>>> createEdgeAsync,
    //     Func<IReadOnlyList<IEdge<TTarget>>, ConnectionPageInfo, int, Connection<TTarget>> createConnection
    // )
    //     where TTarget : class
    //     where TSource : class
    // {
    //     return await CreateConnectionAsync(await pagePromise, createEdgeAsync, createConnection);
    // }

    // [Pure]
    // private static async Task<Connection<TTarget>> CreateConnectionAsync<TSource, TTarget>(
    //     Page<TSource>? page,
    //     Func<Page<TSource>, PageEntry<TSource>, Task<IEdge<TTarget>>> createEdgeAsync,
    //     Func<IReadOnlyList<IEdge<TTarget>>, ConnectionPageInfo, int, Connection<TTarget>> createConnection
    // )
    //     where TTarget : class
    // {
    //     page ??= Page<TSource>.Empty;
    //     var entries = page.Entries;
    //     IEdge<TTarget>[] edges = entries.IsEmpty
    //         ? []
    //         : await Task.WhenAll(
    //               entries
    //                   .Select(entry => createEdgeAsync(page, entry))
    //                   .ToList()
    //           );
    //     return createConnection(
    //         edges,
    //         new ConnectionPageInfo(
    //             page.HasNextPage,
    //             page.HasPreviousPage,
    //             page.CreateStartCursor(),
    //             page.CreateEndCursor()),
    //         page.TotalCount ?? 0);
    // }
}