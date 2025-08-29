using System;
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
using Metabase.Services;
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
        var normalizedFingerprint = GnuPgKeyFingerprint.Normalize(input.Fingerprint);
        if (!await authorization.IsAuthorizedToAdd(
                claimsPrincipal,
                input.InstitutionId,
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

        if (await context.GnuPgKeyFingerprints.AsQueryable()
                .Where(f => f.Fingerprint == normalizedFingerprint)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new AddGnuPgKeyFingerprintError(
                    AddGnuPgKeyFingerprintErrorCode.DUPLICATE_FINGERPRINT,
                    $"The normalized fingerprint {normalizedFingerprint} does already exist.",
                    [nameof(input), nameof(input.Fingerprint).FirstCharToLower()]
                )
            );
        }

        if (!await GnuPgService.DoesGnuPgKeyExist(normalizedFingerprint))
        {
            errors.Add(
                new AddGnuPgKeyFingerprintError(
                    AddGnuPgKeyFingerprintErrorCode.UNKNOWN_KEY,
                    $"There is no non-revoked and non-disabled key in the keyserver {GnuPgService.KeyServerUrl} with the normalized fingerprint {normalizedFingerprint}.",
                    [nameof(input), nameof(input.Fingerprint).FirstCharToLower()]
                )
            );
        }

        if (errors.Count is not 0)
        {
            return new AddGnuPgKeyFingerprintPayload(errors.AsReadOnly());
        }

        var user = await authorization.GetUserAsync(claimsPrincipal)
            ?? throw new InvalidOperationException("Could not obtain the current user.");
        var fingerprint = new GnuPgKeyFingerprint(normalizedFingerprint)
        {
            InstitutionId = input.InstitutionId,
            UserId = user.Id,
        };
        if (await authorization.IsAuthorizedToAllow(
                claimsPrincipal,
                fingerprint,
                cancellationToken
            )
        )
        {
            fingerprint.Allow();
        }
        context.GnuPgKeyFingerprints.Add(fingerprint);
        await context.SaveChangesAsync(cancellationToken);
        return new AddGnuPgKeyFingerprintPayload(fingerprint);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<AllowGnuPgKeyFingerprintPayload> AllowGnuPgKeyFingerprintAsync(
        AllowGnuPgKeyFingerprintInput input,
        ClaimsPrincipal claimsPrincipal,
        GnuPgKeyFingerprintAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var fingerprint = await context.GnuPgKeyFingerprints.AsQueryable()
                .SingleOrDefaultAsync(f =>
                    f.Fingerprint == GnuPgKeyFingerprint.Normalize(input.Fingerprint),
                    cancellationToken
                );
        if (fingerprint is null)
        {
            return new AllowGnuPgKeyFingerprintPayload(
                new AllowGnuPgKeyFingerprintError(
                    AllowGnuPgKeyFingerprintErrorCode.UNKNOWN_FINGERPRINT,
                    "Unknown GnuPG key fingerprint.",
                    []
                )
            );
        }
        if (!await authorization.IsAuthorizedToAllow(
                claimsPrincipal,
                fingerprint,
                cancellationToken
            )
           )
        {
            return new AllowGnuPgKeyFingerprintPayload(
                new AllowGnuPgKeyFingerprintError(
                    AllowGnuPgKeyFingerprintErrorCode.UNAUTHORIZED,
                    "You are not authorized to allow the GnuPG key fingerprint for signing.",
                    [nameof(input), nameof(input.Fingerprint).FirstCharToLower()]
                )
            );
        }
        fingerprint.Allow();
        await context.SaveChangesAsync(cancellationToken);
        return new AllowGnuPgKeyFingerprintPayload(fingerprint);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<ForbidGnuPgKeyFingerprintPayload> ForbidGnuPgKeyFingerprintAsync(
        ForbidGnuPgKeyFingerprintInput input,
        ClaimsPrincipal claimsPrincipal,
        GnuPgKeyFingerprintAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var fingerprint = await context.GnuPgKeyFingerprints.AsQueryable()
                .SingleOrDefaultAsync(f =>
                    f.Fingerprint == GnuPgKeyFingerprint.Normalize(input.Fingerprint),
                    cancellationToken
                );
        if (fingerprint is null)
        {
            return new ForbidGnuPgKeyFingerprintPayload(
                new ForbidGnuPgKeyFingerprintError(
                    ForbidGnuPgKeyFingerprintErrorCode.UNKNOWN_FINGERPRINT,
                    "Unknown GnuPG key fingerprint.",
                    []
                )
            );
        }
        if (!await authorization.IsAuthorizedToForbid(
                claimsPrincipal,
                fingerprint,
                cancellationToken
            )
           )
        {
            return new ForbidGnuPgKeyFingerprintPayload(
                new ForbidGnuPgKeyFingerprintError(
                    ForbidGnuPgKeyFingerprintErrorCode.UNAUTHORIZED,
                    "You are not authorized to forbid the GnuPG key fingerprint.",
                    [nameof(input), nameof(input.Fingerprint).FirstCharToLower()]
                )
            );
        }
        fingerprint.Forbid();
        await context.SaveChangesAsync(cancellationToken);
        return new ForbidGnuPgKeyFingerprintPayload(fingerprint);
    }
}