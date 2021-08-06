using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization
{
    public static class CommonAuthorization
    {
        public static async Task<bool> IsSame(
            ClaimsPrincipal claimsPrincipal,
            Guid userId,
            UserManager<Data.User> userManager
        )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return false;
            }
            return user.Id == userId;
        }

        public static Task<bool> IsAdministrator(
            ClaimsPrincipal claimsPrincipal,
            UserManager<Data.User> userManager
        )
        {
            return IsInRole(
                claimsPrincipal,
                Data.Role.Administrator,
                userManager
            );
        }

        public static Task<bool> IsVerifier(
            ClaimsPrincipal claimsPrincipal,
            UserManager<Data.User> userManager
        )
        {
            return IsInRole(
                claimsPrincipal,
                Data.Role.Verifier,
                userManager
            );
        }

        private static async Task<bool> IsInRole(
            ClaimsPrincipal claimsPrincipal,
            string role,
            UserManager<Data.User> userManager
        )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return false;
            }
            return await userManager.IsInRoleAsync(
                user,
                role
            ).ConfigureAwait(false);
        }

        public static async Task<bool> IsVerified(
            Guid institutionId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return await context.Institutions.AsQueryable()
                .AnyAsync(x =>
                    x.Id == institutionId &&
                    x.State == Enumerations.InstitutionState.VERIFIED,
                    cancellationToken
                ).ConfigureAwait(false);
        }

        public static async Task<bool> IsOwner(
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

        public static async Task<bool> IsOwnerOfVerifiedInstitution(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return
                (await IsVerified(
                    institutionId,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)) &&
                (await IsOwner(
                    claimsPrincipal,
                    institutionId,
                    userManager,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)
            );
        }

        public static async Task<bool> IsAtLeastAssistant(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            var role =
                await FetchRole(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            ).ConfigureAwait(false);
            return
                role == Enumerations.InstitutionRepresentativeRole.OWNER
                || role == Enumerations.InstitutionRepresentativeRole.ASSISTANT;
        }

        public static async Task<bool> IsAtLeastAssistantOfVerifiedInstitution(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return
                (await IsVerified(
                    institutionId,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)) &&
                (await IsAtLeastAssistant(
                    claimsPrincipal,
                    institutionId,
                    userManager,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)
            );
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
                await context.InstitutionRepresentatives.AsQueryable()
                .Where(x =>
                    x.InstitutionId == institutionId &&
                    x.UserId == user.Id &&
                    !x.Pending
                    )
                .Select(x => new { x.Role }) // We wrap the role in an object whose default value is `null`. Note that enumerations have the first value as default value.
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (wrappedRole is not null)
            {
                return wrappedRole.Role;
            }
            var wrappedManagerRole =
                await context.InstitutionRepresentatives.AsQueryable()
                .Where(x => !x.Pending)
                .Join(
                    context.Institutions,
                    representative => representative.InstitutionId,
                    institution => institution.ManagerId,
                    (representative, institution) => new { Representative = representative, Institution = institution }
                )
                .Where(x =>
                    x.Institution.Id == institutionId &&
                    x.Representative.UserId == user.Id
                    )
                .Select(x => new { x.Representative.Role }) // We wrap the role in an object whose default value is `null`. Note that enumerations have the first value as default value.
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            return wrappedManagerRole?.Role;
        }
    }
}