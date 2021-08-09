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
    }
}