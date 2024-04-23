using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class OpenIdConnectAuthorization
    {
        public static async Task<bool> IsAuthorizedToView(
            ClaimsPrincipal claimsPrincipal,
            UserManager<Data.User> userManager
        )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            return (user is not null)
                   && await CommonAuthorization.IsAdministrator(
                       user,
                       userManager
                   );
        }
    }
}