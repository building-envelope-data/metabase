using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class ComponentManufacturerAuthorization
    {
        public static Task<bool> IsAuthorizedToConfirm(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            return CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            );
        }
    }
}