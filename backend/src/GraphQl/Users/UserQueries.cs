using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Users
{
    [ExtendObjectType(nameof(Query))]
    public sealed class UserQueries
    {
        [UseUserManager]
        public async Task<Data.User?> GetCurrentUserAsync(
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager
            )
        {
            return await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        }

        [UsePaging]
        /* TODO [UseProjection] // fails without an explicit error message in the logs */
        /* TODO [UseFiltering(typeof(UserFilterType))] // wait for https://github.com/ChilliCream/hotchocolate/issues/2672 and https://github.com/ChilliCream/hotchocolate/issues/2666 */
        [UseSorting]
        public IQueryable<Data.User> GetUsers(
            Data.ApplicationDbContext context
            )
        {
            return context.Users;
        }

        public Task<Data.User?> GetUserAsync(
            Guid uuid,
            UserByIdDataLoader userById,
            CancellationToken cancellationToken
            )
        {
            return userById.LoadAsync(
                uuid,
                cancellationToken
                );
        }
    }
}