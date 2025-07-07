using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserRole = Metabase.Enumerations.UserRole;

namespace Metabase.Authorization;

public abstract class CommonAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
    )
{
    protected ApplicationDbContext Context { get; } = context;
    protected UserManager<User> UserManager { get; } = userManager;

    internal Task<User?> GetUserAsync(ClaimsPrincipal claimsPrincipal)
    {
        return UserManager.GetUserAsync(claimsPrincipal);
    }

    protected bool IsSame(
        User user,
        Guid userId
    )
    {
        return user.Id == userId;
    }

    protected Task<bool> IsAdministrator(
        User user
    )
    {
        return IsInRole(
            user,
            UserRole.ADMINISTRATOR
        );
    }

    protected Task<bool> IsVerifier(
        User user
    )
    {
        return IsInRole(
            user,
            UserRole.VERIFIER
        );
    }

    protected async Task<bool> IsOwner(
        User user,
        CancellationToken cancellationToken
    )
    {
        var roles = await FetchRoles(
                   user,
                   cancellationToken
               );
        return roles.Contains(InstitutionRepresentativeRole.OWNER);
    }

    protected async Task<bool> IsAtLeastAssistant(
        User user,
        CancellationToken cancellationToken
    )
    {
        var roles = await FetchRoles(
                   user,
                   cancellationToken
               );
        return roles.Contains(InstitutionRepresentativeRole.OWNER) || roles.Contains(InstitutionRepresentativeRole.ASSISTANT);
    }

    internal Task<bool> IsInRole(
        User user,
        UserRole role
    )
    {
        return UserManager.IsInRoleAsync(
            user,
            Role.EnumToName(role)
        );
    }

    internal Task<IList<User>> GetUsersInRoleAsync(
        UserRole role
    )
    {
        return UserManager.GetUsersInRoleAsync(
            Role.EnumToName(role)
        );
    }

    protected Task<bool> IsVerified(
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return Context.Institutions.AsNoTracking()
            .AnyAsync(x =>
                    x.Id == institutionId &&
                    x.State == InstitutionState.VERIFIED,
                cancellationToken
            );
    }

    protected async Task<bool> IsOwnerOfInstitution(
        User user,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return await FetchRole(
                   user,
                   institutionId,
                   cancellationToken
               ).ConfigureAwait(false)
               == InstitutionRepresentativeRole.OWNER;
    }

    protected async Task<bool> IsOwnerOfVerifiedInstitution(
        User user,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return await IsVerified(
                institutionId,
                cancellationToken
            ).ConfigureAwait(false) &&
            await IsOwnerOfInstitution(
                user,
                institutionId,
                cancellationToken
            );
    }

    protected async Task<bool> IsAtLeastAssistant(
        User user,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var role = await FetchRole(
                user,
                institutionId,
                cancellationToken
            );
        return
            role is InstitutionRepresentativeRole.OWNER
            or InstitutionRepresentativeRole.ASSISTANT;
    }

    protected async Task<bool> IsAtLeastAssistantOfVerifiedInstitution(
        User user,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return await IsVerified(
                institutionId,
                cancellationToken
            ).ConfigureAwait(false) &&
            await IsAtLeastAssistant(
                user,
                institutionId,
                cancellationToken
            );
    }

    private async Task<InstitutionRepresentativeRole?> FetchRole(
        User user,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var wrappedRole = await Context.InstitutionRepresentatives.AsNoTracking()
                .Where(x =>
                    x.InstitutionId == institutionId &&
                    x.UserId == user.Id &&
                    !x.Pending
                )
                .Select(x => new
                {
                    x.Role
                }) // We wrap the role in an object whose default value is `null`. Note that enumerations have the first value as default value.
                .SingleOrDefaultAsync(cancellationToken);
        if (wrappedRole is not null)
        {
            return wrappedRole.Role;
        }
        // TODO Recursively fetch manager roles (currently we support only one level)
        var wrappedManagerRole =
            await Context.InstitutionRepresentatives.AsNoTracking()
                .Where(x => !x.Pending)
                .Join(
                    Context.Institutions,
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
                .SingleOrDefaultAsync(cancellationToken);
        return wrappedManagerRole?.Role;
    }

    private async Task<IReadOnlyList<InstitutionRepresentativeRole>> FetchRoles(
        User user,
        CancellationToken cancellationToken
    )
    {
        return (await Context.InstitutionRepresentatives.AsNoTracking()
                .Where(x => x.UserId == user.Id && !x.Pending)
                .Select(x => x.Role)
                .ToListAsync(cancellationToken)
                )
                .AsReadOnly();
    }

    protected Task<bool> IsVerifiedManufacturerOfComponents(
        Guid institutionId,
        Guid[] componentIds,
        CancellationToken cancellationToken
    )
    {
        if (componentIds.Length == 0)
        {
            return Task.FromResult(true);
        }
        return Context.ComponentManufacturers.AsNoTracking()
            .AnyAsync(x =>
                    x.InstitutionId == institutionId &&
                    componentIds.Contains(x.ComponentId) &&
                    !x.Pending,
                cancellationToken
            );
    }

    protected Task<bool> IsVerifiedManufacturerOfComponent(
        Guid institutionId,
        Guid componentId,
        CancellationToken cancellationToken
    )
    {
        return Context.ComponentManufacturers.AsNoTracking()
            .AnyAsync(x =>
                    x.InstitutionId == institutionId &&
                    x.ComponentId == componentId &&
                    !x.Pending,
                cancellationToken
            );
    }
}