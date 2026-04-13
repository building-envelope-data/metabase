using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;
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
    /* [UseProjection] // fails without an explicit error message in the logs */
    [UseFiltering<UserFilterType>]
    [UseSorting<UserSortType>]
    public IQueryable<User> GetUsers(
        ApplicationDbContext context,
        ISortingContext sorting
    )
    {
        sorting.StabilizeOrder<User>();
        return context.Users.AsNoTracking();
    }

    public Task<User?> GetUserAsync(
        Guid id,
        UserByIdDataLoader userById,
        CancellationToken cancellationToken
    )
    {
        return userById.LoadAsync(
            id,
            cancellationToken
        );
    }
}