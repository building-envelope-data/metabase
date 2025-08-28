using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

[ExtendObjectType(nameof(Mutation))]
public sealed class GnuPgKeyFingerprintMutations
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<AddGnuPgKeyFingerprintPayload> AddGnuPgKeyFingerprintAsync(
        GnuPgKeyFingerprintInput input,
        ClaimsPrincipal claimsPrincipal,
        GnuPgKeyFingerprintAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToAdd(
                claimsPrincipal,
                input.InstitutionId,
                input.UserId,
                cancellationToken
            )
           )
        {
            return new AddGnuPgKeyFingerprintPayload(
                new AddGnuPgKeyFingerprintError(
                    AddGnuPgKeyFingerprintErrorCode.UNAUTHORIZED,
                    "You are not authorized to add GnuPG key fingerprints.",
                    []
                )
            );
        }

        var errors = new List<AddGnuPgKeyFingerprintError>();
        if (!await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new AddGnuPgKeyFingerprintError(
                    AddGnuPgKeyFingerprintErrorCode.UNKNOWN_INSTITUTION,
                    "Unknown institution.",
                    [nameof(input), nameof(input.InstitutionId).FirstCharToLower()]
                )
            );
        }

        if (!await context.Users.AsQueryable()
                .Where(u => u.Id == input.UserId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new AddGnuPgKeyFingerprintError(
                    AddGnuPgKeyFingerprintErrorCode.UNKNOWN_USER,
                    "Unknown user.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                )
            );
        }

        if (!await context.GnuPgKeyFingerprints.AsQueryable()
                .Where(f => f.Fingerprint == input.Fingerprint)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new AddGnuPgKeyFingerprintError(
                    AddGnuPgKeyFingerprintErrorCode.DUPLICATE_FINGERPRINT,
                    "The fingerprint does already exist.",
                    [nameof(input), nameof(input.Fingerprint).FirstCharToLower()]
                )
            );
        }

        if (errors.Count is not 0)
        {
            return new AddGnuPgKeyFingerprintPayload(errors.AsReadOnly());
        }

        var fingerprint = new GnuPgKeyFingerprint(input.Fingerprint);
        context.GnuPgKeyFingerprints.Add(fingerprint);
        await context.SaveChangesAsync(cancellationToken);
        return new AddGnuPgKeyFingerprintPayload(fingerprint);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<RevokeGnuPgKeyFingerprintPayload> RevokeGnuPgKeyFingerprintAsync(
        RevokeGnuPgKeyFingerprintInput input,
        ClaimsPrincipal claimsPrincipal,
        GnuPgKeyFingerprintAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var fingerprint = await context.GnuPgKeyFingerprints.AsQueryable()
                .SingleOrDefaultAsync(f =>
                    f.Fingerprint == input.Fingerprint,
                    cancellationToken
                );
        if (fingerprint is null)
        {
            return new RevokeGnuPgKeyFingerprintPayload(
                new RevokeGnuPgKeyFingerprintError(
                    RevokeGnuPgKeyFingerprintErrorCode.UNKNOWN_FINGERPRINT,
                    "Unknown GnuPG key fingerprint.",
                    []
                )
            );
        }
        if (!await authorization.IsAuthorizedToRevoke(
                claimsPrincipal,
                fingerprint.InstitutionId,
                fingerprint.UserId,
                cancellationToken
            )
           )
        {
            return new RevokeGnuPgKeyFingerprintPayload(
                new RevokeGnuPgKeyFingerprintError(
                    RevokeGnuPgKeyFingerprintErrorCode.UNAUTHORIZED,
                    "You are not authorized to revoke the GnuPG key fingerprint.",
                    [nameof(input), nameof(input.Fingerprint).FirstCharToLower()]
                )
            );
        }
        fingerprint.Revoke();
        await context.SaveChangesAsync(cancellationToken);
        return new RevokeGnuPgKeyFingerprintPayload(fingerprint);
    }
}