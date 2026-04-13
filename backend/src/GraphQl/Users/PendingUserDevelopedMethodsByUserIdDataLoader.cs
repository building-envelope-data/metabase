using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users;

public sealed class PendingUserDevelopedMethodsByUserIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
        : AssociationsByAssociateIdDataLoader<UserMethodDeveloper>(
        batchScheduler,
        options,
        dbContextFactory,
        (dbContext, ids, queryContext) =>
                dbContext.UserMethodDevelopers.AsNoTracking().Where(x =>
                    x.Pending && ids.Contains(x.UserId)
                ).With(queryContext),
        x => x.UserId
        )
{
}