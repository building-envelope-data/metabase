using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

[ExtendObjectType(nameof(Query))]
public sealed class GnuPgKeyFingerprintQueries
{
    public Task<GnuPgKeyFingerprint?> GetGnuPgKeyFingerprintAsync(
        string fingerprint,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return context.GnuPgKeyFingerprints.AsQueryable()
            .SingleOrDefaultAsync(f =>
                f.Fingerprint == fingerprint,
                cancellationToken
            );
    }

    public async Task<bool> VerifyGnuPgKeyFingerprintAsync(
        string fingerpint,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var fingerprint = await context.GnuPgKeyFingerprints.AsQueryable()
                .SingleOrDefaultAsync(f =>
                    f.Fingerprint == fingerpint,
                    cancellationToken
                );
        return fingerprint is not null
            && !fingerprint.IsRevoked;
    }
}