using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Users
{
    [ExtendObjectType(Name = nameof(Query))]
    public sealed class UserQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<Data.User?> GetCurrentUserAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager
            )
        {
            return await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        /* TODO [UseProjection] // fails without an explicit error message in the logs */
        /* TODO [UseFiltering(typeof(UserFilterType))] // wait for https://github.com/ChilliCream/hotchocolate/issues/2672 and https://github.com/ChilliCream/hotchocolate/issues/2666 */
        [UseSorting]
        public IQueryable<Data.User> GetUsers(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
            return context.Users;
        }
    }
}