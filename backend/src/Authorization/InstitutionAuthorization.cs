using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class InstitutionAuthorization
    {
        public static Task<bool> IsAuthorizedToUpdateInstitution(
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

        public static Task<bool> IsAuthorizedToDeleteInstitution(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            return CommonAuthorization.IsOwner(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            );
        }

        public static Task<bool> IsAuthorizedToManageRepresentatives(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            return CommonAuthorization.IsOwner(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            );
        }
    }
}