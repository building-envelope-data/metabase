using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users;

public sealed class PendingUserRepresentedInstitutionsByUserIdDataLoader
    : AssociationsByAssociateIdDataLoader<InstitutionRepresentative>
{
    public PendingUserRepresentedInstitutionsByUserIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
        : base(
            batchScheduler,
            options,
            dbContextFactory,
            (dbContext, ids) =>
                dbContext.InstitutionRepresentatives.AsQueryable().Where(x =>
                    x.Pending && ids.Contains(x.UserId)
                ),
            x => x.UserId
        )
    {
    }
}