using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization
{
    public static class ComponentAssemblyAuthorization
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
                await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                    user,
                    componentId,
                    context,
                    cancellationToken
                );
        }

        public static async Task<bool> IsAuthorizedToManage(
            ClaimsPrincipal claimsPrincipal,
            Guid assembledComponentId,
            Guid partComponentId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (user is not null)
                &&
                await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                    user,
                    assembledComponentId,
                    context,
                    cancellationToken
                )
                &&
                await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                    user,
                    partComponentId,
                    context,
                    cancellationToken
                );
        }

        private static async Task<bool> IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
            Data.User user,
            Guid componentId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            var manufacturerIds =
                await context.Institutions.AsQueryable()
                .Where(i => i.ManufacturedComponents.Any(c => c.Id == componentId))
                .Select(i => i.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var manufacturerId in manufacturerIds)
            {
                if (await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                        user,
                        manufacturerId,
                        context,
                        cancellationToken
                    ).ConfigureAwait(false)
                    &&
                    await CommonAuthorization.IsVerifiedManufacturerOfComponent(
                        manufacturerId,
                        componentId,
                        context,
                        cancellationToken
                    ).ConfigureAwait(false)
                )
                {
                    return true;
                }
            }
            return false;
        }
    }
}