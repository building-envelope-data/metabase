using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            return IsMaintainer(
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
            return IsOwner(
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
            return IsOwner(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            );
        }

        private static async Task<bool> IsOwner(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return await FetchRole(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            ).ConfigureAwait(false)
            == Enumerations.InstitutionRepresentativeRole.OWNER;
        }

        private static async Task<bool> IsMaintainer(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return await FetchRole(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            ).ConfigureAwait(false)
            == Enumerations.InstitutionRepresentativeRole.MAINTAINER;
        }

        private static async Task<Enumerations.InstitutionRepresentativeRole?> FetchRole(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return null;
            }
            var wrappedRole =
                await context.InstitutionRepresentatives
                .Where(x =>
                    x.InstitutionId == institutionId &&
                    x.UserId == user.Id
                    )
                .Select(x => new { x.Role }) // We wrap the role in an object whose default value is `null`. Note that enumerations have the first value as default value.
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            return wrappedRole?.Role;
        }
    }
}