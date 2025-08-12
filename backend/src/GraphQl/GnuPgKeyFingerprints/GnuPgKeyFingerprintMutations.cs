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
        InstitutionRepresentativeAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToAddGnuPgKeyFingerprint(
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
                    "You are not authorized to add key fingerprints.",
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

        if (errors.Count is not 0)
        {
            return new AddGnuPgKeyFingerprintPayload(errors.AsReadOnly());
        }

        var institutionRepresentative = await context.InstitutionRepresentatives
                .FirstOrDefaultAsync(r =>
                    r.InstitutionId == input.InstitutionId
                    && r.UserId == input.UserId
                , cancellationToken);

        if (institutionRepresentative is null)
        {
            return new AddGnuPgKeyFingerprintPayload(new AddGnuPgKeyFingerprintError(
                    AddGnuPgKeyFingerprintErrorCode.UNKNOWN_REPRESENTATIVE,
                    "Unknown representative.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                ));
        }

        if (institutionRepresentative.DataSigningPermission is not Enumerations.DataSigningPermission.ALLOWED)
        {
            return new AddGnuPgKeyFingerprintPayload(new AddGnuPgKeyFingerprintError(
                    AddGnuPgKeyFingerprintErrorCode.NOT_ALLOWED,
                    "Representative is not allowed to sign data.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                ));
        }

        institutionRepresentative.GnuPgKeyFingerprints.Add(input.Fingerprint);
        await context.SaveChangesAsync(cancellationToken);
        return new AddGnuPgKeyFingerprintPayload(input.Fingerprint);
    }
}