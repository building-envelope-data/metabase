using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class UserMethodDeveloperAuthorization
    {
        public static Task<bool> IsAuthorizedToConfirm(
            ClaimsPrincipal claimsPrincipal,
            Guid userId,
            UserManager<Data.User> userManager
            )
        {
            return CommonAuthorization.IsSame(
                claimsPrincipal,
                userId,
                userManager
            );
        }
    }
}