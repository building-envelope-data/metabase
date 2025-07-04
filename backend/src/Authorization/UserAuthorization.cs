using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Metabase.Data;
using UserRole = Metabase.Enumerations.UserRole;

namespace Metabase.Authorization;

public sealed class UserAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    internal Task<bool> HasPasswordAsync(User user)
    {
        return UserManager.HasPasswordAsync(user);
    }

    internal async Task<IEnumerable<UserRole>> GetRolesAsync(User user)
    {
        return (await UserManager.GetRolesAsync(user)).Select(Role.EnumFromName);
    }

    internal async Task<bool> IsAuthorizedToDeleteUsers(
        ClaimsPrincipal claimsPrincipal
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsAdministrator(user);
    }

    internal async Task<bool> IsAuthorizedToManageUser(
        ClaimsPrincipal claimsPrincipal,
        Guid userId
    )
    {
        var loggedInUser = await GetUserAsync(claimsPrincipal);
        if (loggedInUser is null)
        {
            return false;
        }

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
    }

    internal async Task<bool> IsAuthorizedToAddOrRemoveRole(
        ClaimsPrincipal claimsPrincipal,
        UserRole role
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        if (user is null)
        {
            return false;
        }

        if (await IsAdministrator(user).ConfigureAwait(false))
        {
            return true;
        }

        return role switch
        {
            UserRole.ADMINISTRATOR =>
                await IsAdministrator(user).ConfigureAwait(false),
            UserRole.VERIFIER =>
                await IsVerifier(user).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(role), $"Unknown role `{role}.`")
        };
    }
}