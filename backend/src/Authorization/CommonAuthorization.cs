using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using static OpenIddict.Abstractions.OpenIddictConstants;
using UserRole = Metabase.Enumerations.UserRole;

namespace Metabase.Authorization;

public abstract class CommonAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
)
: IDisposable, IAsyncDisposable
{
    // This is the same code that HotChocolate returns when autheorization via the attribute `[Authorize(Policy = ...)]` fails.
    private const string UNAUTHORIZED_CODE = "AUTH_NOT_AUTHENTICATED";

    protected ApplicationDbContext Context { get; } = dbContextFactory.CreateDbContext();
    protected UserManager<User> UserManager { get; } = userManager;
    protected OpenIddictApplicationManager<OpenIdConnectApplication> ApplicationManager { get; } = applicationManager;

    // [Implement a DisposeAsync method](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync)
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Context.Dispose();
        }
    }

    protected virtual ValueTask DisposeAsyncCore()
    {
        return Context.DisposeAsync();
    }

    public Task<T> SwitchUserOrApplicationAsync<T>(
        ClaimsPrincipal claimsPrincipal,
        Func<User?, Task<T>> handleUser,
        Func<OpenIdConnectApplication?, Task<T>> handleApplication,
        CancellationToken cancellationToken
    )
    {
        return IOpenIdConnectSubject.SwitchSubjectAsync(
            claimsPrincipal.GetClaim(Claims.Subject),
            async (_) => await handleUser(
                await GetUserAsync(claimsPrincipal)
            ),
            async (clientId) => await handleApplication(
                await ApplicationManager.FindByClientIdAsync(clientId, cancellationToken)
            ),
            async () => await handleUser(
                await GetUserAsync(claimsPrincipal)
            )
        );
    }

    protected Task<bool> AuthorizeAsync(
        ClaimsPrincipal claimsPrincipal,
        Func<User, Task<bool>> authorizeUser,
        Func<OpenIdConnectApplication, Task<bool>> authorizeApplication,
        CancellationToken cancellationToken
    )
    {
        return SwitchUserOrApplicationAsync(
            claimsPrincipal,
            async user => user is not null && (
                await CanAdministrate(user, claimsPrincipal)
                || await authorizeUser(user)
            ),
            async application => application is not null &&
                await authorizeApplication(application),
            cancellationToken
        );
    }

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

    public Task<bool> CanAdministrate(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => CanAdministrate(user, claimsPrincipal),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    public async Task<bool> CanAdministrate(
        User user,
        ClaimsPrincipal claimsPrincipal
    )
    {
        return
            claimsPrincipal.HasScope(OpenIdConnectScope.AdministrateApiScope)
            && await IsInRole(
                user,
                UserRole.ADMINISTRATOR
            );
    }

    public async Task<bool> CanVerify(
        User user,
        ClaimsPrincipal claimsPrincipal
    )
    {
        return
            claimsPrincipal.HasScope(OpenIdConnectScope.VerifyApiScope)
            && await IsInRole(
                user,
                UserRole.VERIFIER
            );
    }

    public async Task<bool> CanSupport(
        User user,
        ClaimsPrincipal claimsPrincipal
    )
    {
        return
            claimsPrincipal.HasScope(OpenIdConnectScope.SupportApiScope)
            && await IsInRole(
                user,
                UserRole.SUPPORTER
            );
    }

    private Task<bool> IsInRole(
        User user,
        UserRole role
    )
    {
        return UserManager.IsInRoleAsync(
            user,
            Role.EnumToName(role)
        );
    }

    internal async Task<IReadOnlyList<User>> GetUsersInRoleAsync(
        UserRole role
    )
    {
        return (await UserManager.GetUsersInRoleAsync(
            Role.EnumToName(role)
        )).AsReadOnly();
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
               )
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
            ) &&
            await IsOwnerOfInstitution(
                user,
                institutionId,
                cancellationToken
            );
    }

    protected async Task<bool> IsAtLeastAssistantOfInstitution(
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
            ) &&
            await IsAtLeastAssistantOfInstitution(
                user,
                institutionId,
                cancellationToken
            );
    }

    private IQueryable<Institution> BelongsToInstitutionQuery(
        OpenIdConnectApplication application,
        Guid institutionId
    )
    {
        return Context.Institutions.AsNoTracking()
            .Where(i => i.Id == institutionId)
            .Where(i =>
                i.Id == application.OwnerId
                || i.ManagerId == application.OwnerId
                || i.Manager != null && i.Manager.ManagerId == application.OwnerId
            );
    }

    protected Task<bool> BelongsToInstitution(
        OpenIdConnectApplication application,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return BelongsToInstitutionQuery(application, institutionId)
            .AnyAsync(cancellationToken);
    }

    protected Task<bool> BelongsToVerifiedInstitution(
        OpenIdConnectApplication application,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return BelongsToInstitutionQuery(application, institutionId)
            .Where(i => i.State == InstitutionState.VERIFIED)
            .AnyAsync(cancellationToken);
    }

    private async Task<InstitutionRepresentativeRole?> FetchRole(
        User user,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var context = Context;
        var wrappedRole = await context.InstitutionRepresentatives.AsNoTracking()
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
            await context.InstitutionRepresentatives.AsNoTracking()
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
                .SingleOrDefaultAsync(cancellationToken);
        return wrappedManagerRole?.Role;
    }

    public void ReportUnauthorizedError(
        IResolverContext resolverContext
    )
    {
        resolverContext.ReportError(
            ErrorBuilder.New()
                .SetCode(UNAUTHORIZED_CODE)
                .SetPath(resolverContext.Path)
                .SetMessage($"The current user is not authorized to access this resource.")
                .Build()
        );
    }
}