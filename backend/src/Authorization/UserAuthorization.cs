using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using UserRole = Metabase.Enumerations.UserRole;

namespace Metabase.Authorization;

public sealed class UserAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(dbContextFactory, userManager, applicationManager)
{
    internal Task<bool> HasPasswordAsync(User user)
    {
        return UserManager.HasPasswordAsync(user);
    }

    internal async Task<IReadOnlyList<UserRole>> GetRolesAsync(User user)
    {
        return (await UserManager.GetRolesAsync(user)).Select(Role.EnumFromName).ToList().AsReadOnly();
    }

    internal Task<bool> IsAuthorizedToDeleteUsers(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => Task.FromResult(false),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToManageUser(
        ClaimsPrincipal claimsPrincipal,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            loggedInUser => Task.FromResult(loggedInUser.Id == userId),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToAddOrRemoveRole(
        ClaimsPrincipal claimsPrincipal,
        UserRole role,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user => role switch
            {
                UserRole.ADMINISTRATOR =>
                    await CanAdministrate(user, claimsPrincipal),
                UserRole.VERIFIER =>
                    await CanVerify(user, claimsPrincipal),
                _ => throw new ArgumentOutOfRangeException(nameof(role), $"Unknown role `{role}.`")
            },
            application => Task.FromResult(false),
            cancellationToken
        );
    }
}