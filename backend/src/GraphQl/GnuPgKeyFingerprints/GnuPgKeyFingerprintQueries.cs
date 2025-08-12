using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.Enumerations;
using Metabase.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

[ExtendObjectType(nameof(Query))]
public sealed class GnuPgKeyFingerprintQueries
{
    public async Task<VerifyGnuPgKeyFingerprintPayload> VerifyGnuPgKeyFingerprintAsync(
        GnuPgKeyFingerprintInput input,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var errors = new List<VerifyGnuPgKeyFingerprintError>();
        if (!await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new VerifyGnuPgKeyFingerprintError(
                    VerifyGnuPgKeyFingerprintErrorCode.UNKNOWN_INSTITUTION,
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
                new VerifyGnuPgKeyFingerprintError(
                    VerifyGnuPgKeyFingerprintErrorCode.UNKNOWN_USER,
                    "Unknown user.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                )
            );
        }

        if (errors.Count is not 0)
        {
            return new VerifyGnuPgKeyFingerprintPayload(errors.AsReadOnly());
        }

        var institutionRepresentative = await context.InstitutionRepresentatives
                .FirstOrDefaultAsync(r =>
                    r.InstitutionId == input.InstitutionId
                    && r.UserId == input.UserId
                , cancellationToken);

        if (institutionRepresentative is null)
        {
            return new VerifyGnuPgKeyFingerprintPayload(new VerifyGnuPgKeyFingerprintError(
                    VerifyGnuPgKeyFingerprintErrorCode.UNKNOWN_REPRESENTATIVE,
                    "Unknown representative.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                ));
        }

        if (!institutionRepresentative.GnuPgKeyFingerprints.Contains(input.Fingerprint))
        {
            return new VerifyGnuPgKeyFingerprintPayload(new VerifyGnuPgKeyFingerprintError(
                    VerifyGnuPgKeyFingerprintErrorCode.UNKNOWN_FINGERPRINT,
                    "Unknown keyfingerprint.",
                    [nameof(input), nameof(input.Fingerprint).FirstCharToLower()]
                ));
        }

        return new VerifyGnuPgKeyFingerprintPayload(institutionRepresentative.DataSigningPermission is DataSigningPermission.ALLOWED
            || institutionRepresentative.DataSigningPermission is DataSigningPermission.FORBIDDEN);
    }
}