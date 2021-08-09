using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class UserAuthorization
    {
        public static Task<bool> IsAuthorizedToDeleteUsers(
            ClaimsPrincipal claimsPrincipal,
            UserManager<Data.User> userManager
        )
        {
            return CommonAuthorization.IsAdministrator(claimsPrincipal, userManager);
        }

        public static async Task<bool> IsAuthorizedToManageUser(
            ClaimsPrincipal claimsPrincipal,
            Guid userId,
            UserManager<Data.User> userManager
            )
        {
            var loggedInUser = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (loggedInUser is null)
            {
                return false;
            }
            if (loggedInUser.Id == userId)
            {
                return true;
            }
            if (await userManager.IsInRoleAsync(
                    loggedInUser,
                    Data.Role.EnumToName(Enumerations.UserRole.ADMINISTRATOR)
                ).ConfigureAwait(false))
            {
                return true;
            }
            return false;
        }

        public static async Task<bool> IsAuthorizedToAddOrRemoveRole(
            ClaimsPrincipal claimsPrincipal,
            Enumerations.UserRole role,
            UserManager<Data.User> userManager
        )
        {
            if (await CommonAuthorization.IsAdministrator(claimsPrincipal, userManager).ConfigureAwait(false))
            {
                return true;
            }
            return role switch
            {
                Enumerations.UserRole.ADMINISTRATOR =>
                    await CommonAuthorization.IsAdministrator(claimsPrincipal, userManager).ConfigureAwait(false),
                Enumerations.UserRole.VERIFIER =>
                    await CommonAuthorization.IsVerifier(claimsPrincipal, userManager).ConfigureAwait(false),
                _ => throw new ArgumentOutOfRangeException(nameof(role), $"Unknown role `{role}.`")
            };
        }
    }
}