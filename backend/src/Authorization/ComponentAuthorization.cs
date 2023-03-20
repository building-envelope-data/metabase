using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class ComponentAuthorization
    {
        public static async Task<bool> IsAuthorizedToCreateComponentForInstitution(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (user is not null) &&
                await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                    user,
                    institutionId,
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
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return
                (user is not null)
                &&
                await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                    user,
                    institutionId,
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

        public static async Task<bool> IsAuthorizedToUpdate(
            ClaimsPrincipal claimsPrincipal,
            Guid componentId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (user is not null) &&
                await CommonComponentAuthorization.IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                    user,
                    componentId,
                    context,
                    cancellationToken
                );
        }
    }
}