using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class DatabaseAuthorization
    {
        public static async Task<bool> IsAuthorizedToCreateDatabaseForInstitution(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (user is not null)
            && await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                user,
                institutionId,
                context,
                cancellationToken
            );
        }
    }
}