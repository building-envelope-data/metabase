using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users;

public sealed class GnuPgKeyFingerprintsByUserIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
        : AssociationsByAssociateIdDataLoader<GnuPgKeyFingerprint>(
        batchScheduler,
        options,
        dbContextFactory,
        (dbContext, ids, queryContext) =>
                dbContext.GnuPgKeyFingerprints.AsNoTracking().Where(x =>
                    ids.Contains(x.UserId)
                ).With(queryContext),
        x => x.UserId
        )
{
}