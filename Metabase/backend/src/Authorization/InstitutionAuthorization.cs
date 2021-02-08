using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization
{
    public static class InstitutionAuthorization
    {
        public static async Task<bool> IsAuthorizedToManageRepresentatives(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return false;
            }
            var wrappedRole =
                await context.InstitutionRepresentatives
                .Where(x =>
                    x.InstitutionId == institutionId &&
                    x.UserId == user.Id
                    )
                .Select(x => new { x.Role }) // We wrap the role in an object whose default value is `null`. Note that enumerations have the first value as default value.
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            return
                wrappedRole is not null &&
                wrappedRole.Role == Enumerations.InstitutionRepresentativeRole.OWNER;
        }
    }
}