using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization
{
    public static class InstitutionRepresentativeAuthorization
    {
        public static Task<bool> IsAuthorizedToManage(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            return CommonAuthorization.IsOwner(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            );
        }

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