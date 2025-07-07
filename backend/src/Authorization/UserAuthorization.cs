using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using UserRole = Metabase.Enumerations.UserRole;
using System.Threading;

namespace Metabase.Authorization;

public sealed class UserAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(context, userManager, applicationManager)
{
    internal Task<bool> HasPasswordAsync(User user)
    {
        return UserManager.HasPasswordAsync(user);
    }

    internal async Task<IEnumerable<UserRole>> GetRolesAsync(User user)
    {
        return (await UserManager.GetRolesAsync(user)).Select(Role.EnumFromName);
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
            async loggedInUser =>
            {
                if (loggedInUser.Id == userId)
                {
                    return true;
                }
                if (await IsInRole(
                        loggedInUser,
                        UserRole.ADMINISTRATOR
                    ).ConfigureAwait(false))
                {
                    return true;
                }
                return false;
            },
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
                    await IsAdministrator(user).ConfigureAwait(false),
                UserRole.VERIFIER =>
                    await IsVerifier(user).ConfigureAwait(false),
                _ => throw new ArgumentOutOfRangeException(nameof(role), $"Unknown role `{role}.`")
            },
            application => Task.FromResult(false),
            cancellationToken
        );
    }
}