using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization
{
    public static class CommonAuthorization
    {
        public static bool IsSame(
            Data.User user,
            Guid userId
        )
        {
            if (user is null)
            {
                return false;
            }

            return user.Id == userId;
        }

        public static Task<bool> IsAdministrator(
            Data.User user,
            UserManager<Data.User> userManager
        )
        {
            return IsInRole(
                user,
                Enumerations.UserRole.ADMINISTRATOR,
                userManager
            );
        }

        public static Task<bool> IsVerifier(
            Data.User user,
            UserManager<Data.User> userManager
        )
        {
            return IsInRole(
                user,
                Enumerations.UserRole.VERIFIER,
                userManager
            );
        }

        private static async Task<bool> IsInRole(
            Data.User user,
            Enumerations.UserRole role,
            UserManager<Data.User> userManager
        )
        {
            if (user is null)
            {
                return false;
            }

            return await userManager.IsInRoleAsync(
                user,
                Data.Role.EnumToName(role)
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
            Data.User user,
            Guid institutionId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return await FetchRole(
                       user,
                       institutionId,
                       context,
                       cancellationToken
                   ).ConfigureAwait(false)
                   == Enumerations.InstitutionRepresentativeRole.OWNER;
        }

        public static async Task<bool> IsOwnerOfVerifiedInstitution(
            Data.User user,
            Guid institutionId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return
                (user is not null) &&
                (await IsVerified(
                    institutionId,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)) &&
                (await IsOwner(
                        user,
                        institutionId,
                        context,
                        cancellationToken
                    ).ConfigureAwait(false)
                );
        }

        public static async Task<bool> IsAtLeastAssistant(
            Data.User user,
            Guid institutionId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            var role =
                await FetchRole(
                    user,
                    institutionId,
                    context,
                    cancellationToken
                ).ConfigureAwait(false);
            return
                role == Enumerations.InstitutionRepresentativeRole.OWNER
                || role == Enumerations.InstitutionRepresentativeRole.ASSISTANT;
        }

        public static async Task<bool> IsAtLeastAssistantOfVerifiedInstitution(
            Data.User user,
            Guid institutionId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return
                (user is not null) &&
                (await IsVerified(
                    institutionId,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)) &&
                (await IsAtLeastAssistant(
                        user,
                        institutionId,
                        context,
                        cancellationToken
                    ).ConfigureAwait(false)
                );
        }

        private static async Task<Enumerations.InstitutionRepresentativeRole?> FetchRole(
            Data.User user,
            Guid institutionId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
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
                    .Select(x => new
                    {
                        x.Role
                    }) // We wrap the role in an object whose default value is `null`. Note that enumerations have the first value as default value.
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
                        (representative, institution) => new
                            { Representative = representative, Institution = institution }
                    )
                    .Where(x =>
                        x.Institution.Id == institutionId &&
                        x.Representative.UserId == user.Id
                    )
                    .Select(x => new
                    {
                        x.Representative.Role
                    }) // We wrap the role in an object whose default value is `null`. Note that enumerations have the first value as default value.
                    .SingleOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);
            return wrappedManagerRole?.Role;
        }

        public static async Task<bool> IsVerifiedManufacturerOfComponents(
            Guid institutionId,
            Guid[] componentIds,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            if (componentIds.Length == 0)
            {
                return true;
            }

            return await context.ComponentManufacturers.AsQueryable()
                .AnyAsync(x =>
                        x.InstitutionId == institutionId &&
                        componentIds.Contains(x.ComponentId) &&
                        !x.Pending,
                    cancellationToken
                )
                .ConfigureAwait(false);
        }

        public static async Task<bool> IsVerifiedManufacturerOfComponent(
            Guid institutionId,
            Guid componentId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return await context.ComponentManufacturers.AsQueryable()
                .AnyAsync(x =>
                        x.InstitutionId == institutionId &&
                        x.ComponentId == componentId &&
                        !x.Pending,
                    cancellationToken
                )
                .ConfigureAwait(false);
        }
    }
}