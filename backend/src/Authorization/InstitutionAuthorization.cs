using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization;

public sealed class InstitutionAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(dbContextFactory, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToCreateInstitution(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        // Logged-in users or authenticated OpenId Connect client applications
        // are authorized to create institutions.
        return AuthorizeAsync(
            claimsPrincipal,
            _ => Task.FromResult(true),
            _ => Task.FromResult(true),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToUpdateInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfInstitution(
                user,
                institutionId,
                cancellationToken
            ),
            application => BelongsToInstitution(
                application,
                institutionId,
                cancellationToken
            ),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToDeleteInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsOwnerOfInstitution(
                   user,
                   institutionId,
                   cancellationToken
               ),
            application => BelongsToInstitution(
                application,
                institutionId,
                cancellationToken
            ),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToCreateInstitutionManagedByInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedInstitution(
                   user,
                   institutionId,
                   cancellationToken
               ),
            application => BelongsToVerifiedInstitution(
                application,
                institutionId,
                cancellationToken
            ),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToVerifyInstitution(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            IsVerifier,
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToSwitchInstitutionOperatingState(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsOwnerOfInstitution(
                   user,
                   institutionId,
                   cancellationToken
               ),
            application => BelongsToInstitution(
                application,
                institutionId,
                cancellationToken
            ),
            cancellationToken
        );
    }
}