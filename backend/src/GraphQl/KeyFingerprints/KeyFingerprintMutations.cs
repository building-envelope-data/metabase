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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.KeyFingerprints;

[ExtendObjectType(nameof(Mutation))]
public sealed class KeyFingerprintMutations
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<AddKeyFingerprintPayload> AddKeyFingerprintAsync(
        KeyFingerprintInput input,
        ClaimsPrincipal claimsPrincipal,
        InstitutionRepresentativeAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToAddKeyFingerprint(
                claimsPrincipal,
                input.InstitutionId,
                input.UserId,
                cancellationToken
            )
           )
        {
            return new AddKeyFingerprintPayload(
                new AddKeyFingerprintError(
                    AddKeyFingerprintErrorCode.UNAUTHORIZED,
                    "You are not authorized to add key fingerprints.",
                    []
                )
            );
        }

        var errors = new List<AddKeyFingerprintError>();
        if (!await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new AddKeyFingerprintError(
                    AddKeyFingerprintErrorCode.UNKNOWN_INSTITUTION,
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
                new AddKeyFingerprintError(
                    AddKeyFingerprintErrorCode.UNKNOWN_USER,
                    "Unknown user.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                )
            );
        }

        if (errors.Count is not 0)
        {
            return new AddKeyFingerprintPayload(errors.AsReadOnly());
        }

        var institutionRepresentative = await context.InstitutionRepresentatives
                .FirstOrDefaultAsync(r =>
                    r.InstitutionId == input.InstitutionId
                    && r.UserId == input.UserId
                , cancellationToken);

        if (institutionRepresentative is null)
        {
            return new AddKeyFingerprintPayload(new AddKeyFingerprintError(
                    AddKeyFingerprintErrorCode.UNKNOWN_REPRESENTATIVE,
                    "Unknown representative.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                ));
        }

        if (institutionRepresentative.DataSigningPermission is not Enumerations.DataSigningPermission.ALLOWED)
        {
            return new AddKeyFingerprintPayload(new AddKeyFingerprintError(
                    AddKeyFingerprintErrorCode.NOT_ALLOWED,
                    "Representative is not allowed to sign data.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                ));
        }

        institutionRepresentative.KeyFingerprints.Add(input.KeyFingerprint);
        await context.SaveChangesAsync(cancellationToken);
        return new AddKeyFingerprintPayload(input.KeyFingerprint);
    }
}