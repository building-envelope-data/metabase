using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class UserMethodDeveloperAuthorization
    {
        public static async Task<bool> IsAuthorizedToConfirm(
            ClaimsPrincipal claimsPrincipal,
            Guid userId,
            UserManager<Data.User> userManager
            )
        {
            var loggedInUser = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (loggedInUser is not null)
            && CommonAuthorization.IsSame(
                loggedInUser,
                userId
            );
        }
    }
}