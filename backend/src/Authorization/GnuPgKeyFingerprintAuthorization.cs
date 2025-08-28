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

public sealed class GnuPgKeyFingerprintAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(dbContextFactory, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToAdd(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedInstitution(user, institutionId, cancellationToken),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToAllow(
        ClaimsPrincipal claimsPrincipal,
        GnuPgKeyFingerprint fingerprint,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user => await IsOwnerOfVerifiedInstitution(user, fingerprint.InstitutionId, cancellationToken),
            application => BelongsToInstitution(
                application,
                fingerprint.InstitutionId,
                cancellationToken
            ),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToRevoke(
        ClaimsPrincipal claimsPrincipal,
        GnuPgKeyFingerprint fingerprint,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user => IsSame(user, fingerprint.UserId) || await IsOwnerOfVerifiedInstitution(user, fingerprint.InstitutionId, cancellationToken),
            application => BelongsToInstitution(
                application,
                fingerprint.InstitutionId,
                cancellationToken
            ),
            cancellationToken
        );
    }
}