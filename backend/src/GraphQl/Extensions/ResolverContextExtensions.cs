using GreenDonut.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Sorting;
using HotChocolate.Resolvers;

namespace Metabase.GraphQl.Extensions;

public static class ResolverContextExtensions
{
    // Inspired by https://github.com/ChilliCream/graphql-platform/blob/9ae7220205412203d0a941a6b0cc779e70b02b09/src/HotChocolate/Data/src/Data/QueryContextParameterExpressionBuilder.cs#L76-L86
    // Using `QueryContext<Component> queryContext,` in resolvers starts up the projection engine producing many problems
    public static QueryContext<T> GetQueryContext<T>(this IResolverContext context)
    {
        var selection = context.Selection;
        var filterContext = context.GetFilterContext();
        var sortContext = context.GetSortingContext();
        // TODO Make selection work
        return new QueryContext<T>(
            null, // selection.AsSelector<T>(),
            filterContext?.AsPredicate<T>(),
            sortContext?.AsSortDefinition<T>());
    }

    // Using `PagingArguments pagingArguments,` in resolvers results in the parameter `pagingArguments: PagingArgumentsInput` in the GraphQL schema
    public static PagingArguments GetPagingArguments(this IResolverContext context)
    {
        return new(
            context.ArgumentValue<int?>("first"),
            context.ArgumentValue<string?>("after"),
            context.ArgumentValue<int?>("last"),
            context.ArgumentValue<string?>("before"),
            includeTotalCount: true
        );
    }
}