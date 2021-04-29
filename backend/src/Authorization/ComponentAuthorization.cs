using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class ComponentAuthorization
    {
        public static Task<bool> IsAuthorizedToCreateComponentForInstitution(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            return CommonAuthorization.IsAtLeastMaintainer(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            );
        }
    }
}