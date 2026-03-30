using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users;

[ExtendObjectType(nameof(Query))]
public sealed class UserQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.AuthenticatedPolicy)]
    public Task<User?> GetCurrentUserAsync(
        ClaimsPrincipal claimsPrincipal,
        UserAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.SwitchUserOrApplicationAsync(
            claimsPrincipal,
            user => Task.FromResult(user),
            application => Task.FromResult<User?>(null),
            cancellationToken
        );
    }

    [UsePaging]
    [UseFiltering<UserFilterType>]
    [UseSorting<UserSortType>]
    public ValueTask<HotChocolate.Types.Pagination.Connection<User>> GetUsersAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.Users
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<User>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    public Task<User?> GetUserAsync(
        Guid id,
        IUserByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        return byId.LoadAsync(id, cancellationToken);
    }
}