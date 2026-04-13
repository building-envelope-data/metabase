using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class GnuPgKeyFingerprintByFingerprintDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
    : BatchDataLoader<string, GnuPgKeyFingerprint?>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<string, GnuPgKeyFingerprint?>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken
    )
    {
        await using var dbContext =
            dbContextFactory.CreateDbContext();
        return await dbContext.GnuPgKeyFingerprints.AsNoTrackingWithIdentityResolution()
            .Where(f => keys.Contains(f.Fingerprint))
            .ToDictionaryAsync(
                f => f.Fingerprint,
                f => (GnuPgKeyFingerprint?)f,
                cancellationToken
            );
    }
}