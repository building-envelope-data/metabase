using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class GnuPgKeyFingerprintsByInstitutionIdDataLoader(
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
                    ids.Contains(x.InstitutionId)
                ).With(queryContext),
        x => x.InstitutionId
        )
{
}