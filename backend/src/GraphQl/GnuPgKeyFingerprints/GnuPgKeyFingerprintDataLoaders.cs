using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class GnuPgKeyFingerprintDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, GnuPgKeyFingerprint>> GetGnuPgKeyFingerprintByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<GnuPgKeyFingerprint> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.GnuPgKeyFingerprints,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static async ValueTask<IReadOnlyDictionary<string, GnuPgKeyFingerprint>> GetGnuPgKeyFingerprintByFingerprintAsync(
        IReadOnlyList<string> fingerprints,
        QueryContext<GnuPgKeyFingerprint> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        await using var databaseContext =
            databaseContextFactory.CreateDbContext();
        return await databaseContext.GnuPgKeyFingerprints
            .AsNoTrackingWithIdentityResolution()
            .Where(_ => fingerprints.Contains(_.Fingerprint))
            .With(queryContext, Sorting.DefaultEntityOrder)
            .ToDictionaryAsync(_ => _.Fingerprint, cancellationToken);
    }
}
