using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users;

public sealed class PendingUserRepresentedInstitutionsByUserIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
        : AssociationsByAssociateIdDataLoader<InstitutionRepresentative>(
        batchScheduler,
        options,
        dbContextFactory,
        (dbContext, ids, queryContext) =>
                dbContext.InstitutionRepresentatives.AsNoTracking().Where(x =>
                    x.Pending && ids.Contains(x.UserId)
                ).With(queryContext),
        x => x.UserId
        )
{
}