using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Authorization;

public sealed class UserMethodDeveloperAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonMethodAuthorization(context, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToAdd(
        ClaimsPrincipal claimsPrincipal,
        Guid methodId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedMethodManager(
                   user,
                   methodId,
                   cancellationToken
               ),
            application => BelongsToVerifiedMethodManager(
                   application,
                   methodId,
                   cancellationToken
               ),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToConfirm(
        ClaimsPrincipal claimsPrincipal,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            loggedInUser => Task.FromResult(
                IsSame(
                   loggedInUser,
                   userId
               )
            ),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToRemove(
        ClaimsPrincipal claimsPrincipal,
        Guid methodId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedMethodManager(
                   user,
                   methodId,
                   cancellationToken
               ),
            application => BelongsToVerifiedMethodManager(
                   application,
                   methodId,
                   cancellationToken
               ),
            cancellationToken
        );
    }
}