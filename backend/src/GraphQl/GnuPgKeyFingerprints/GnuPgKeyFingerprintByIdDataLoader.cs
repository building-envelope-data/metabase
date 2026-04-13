using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class GnuPgKeyFingerprintByIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
        : EntityByIdDataLoader<GnuPgKeyFingerprint>(
        batchScheduler,
        options,
        dbContextFactory,
        dbContext => dbContext.GnuPgKeyFingerprints
        )
{
}