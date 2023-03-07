using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class ComponentVariantAuthorization
    {
        public static async Task<bool> IsAuthorizedToAdd(
            ClaimsPrincipal claimsPrincipal,
            Guid componentId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (user is not null)
                &&
                await ComponentReflexiveAssociationAuthorization.IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                    user,
                    componentId,
                    context,
                    cancellationToken
                );
        }

        public static async Task<bool> IsAuthorizedToManage(
            ClaimsPrincipal claimsPrincipal,
            Guid ofComponentId,
            Guid toComponentId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (user is not null)
                &&
                await ComponentReflexiveAssociationAuthorization.IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                    user,
                    ofComponentId,
                    context,
                    cancellationToken
                )
                &&
                await ComponentReflexiveAssociationAuthorization.IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                    user,
                    toComponentId,
                    context,
                    cancellationToken
                );
        }
    }
}