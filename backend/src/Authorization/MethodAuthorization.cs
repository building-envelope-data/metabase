using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization
{
    public static class MethodAuthorization
    {
        public static async Task<bool> IsAuthorizedToCreateMethodManagedByInstitution(
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

        public static async Task<bool> IsAuthorizedToUpdate(
            ClaimsPrincipal claimsPrincipal,
            Guid methodId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (user is not null) &&
                await IsAtLeastAssistantOfVerifiedMethodManager(
                    user,
                    methodId,
                    context,
                    cancellationToken
                );
        }

        private static async Task<bool> IsAtLeastAssistantOfVerifiedMethodManager(
            Data.User user,
            Guid methodId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            var wrappedManagerId =
                await context.Methods.AsQueryable()
                .Where(x => x.Id == methodId)
                .Select(x => new { x.ManagerId })
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (wrappedManagerId is null)
            {
                return false;
            }
            return await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                user, wrappedManagerId.ManagerId, context, cancellationToken
            );
        }
    }
}