using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class OpenIdConnectAuthorization
    {
        public static Task<bool> IsAuthorizedToView(
            ClaimsPrincipal claimsPrincipal,
            UserManager<Data.User> userManager
            )
        {
            return CommonAuthorization.IsAdministrator(
                claimsPrincipal,
                userManager
            );
        }
    }
}