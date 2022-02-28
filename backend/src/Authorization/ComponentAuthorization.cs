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
            return CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            );
        }

        public static async Task<bool> IsAuthorizedToAddAssociationFromNewComponentToExistingComponents(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            Guid[] existingComponentIds,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            return
                await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                    claimsPrincipal,
                    institutionId,
                    userManager,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)
                &&
                await CommonAuthorization.IsVerifiedManufacturerOfComponents(
                    institutionId,
                    existingComponentIds,
                    context,
                    cancellationToken
                ).ConfigureAwait(false);
        }
    }
}