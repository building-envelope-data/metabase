using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
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
            return CommonAuthorization.IsAtLeastAssistant(
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
            return CommonAuthorization.IsOwnerOfVerifiedInstitution(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            );
        }

        public static Task<bool> IsAuthorizedToCreateInstitutionManagedByInstitution(
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

        internal static Task<bool> IsAuthorizedToVerifyInstitution(
            ClaimsPrincipal claimsPrincipal,
            UserManager<Data.User> userManager
            )
        {
            return CommonAuthorization.IsVerifier(
                claimsPrincipal,
                userManager
            );
        }
    }
}