using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users;

public sealed class UserRepresentedInstitutionsByUserIdDataLoader
    : Entities.AssociationsByAssociateIdDataLoader<Data.InstitutionRepresentative>
{
    public UserRepresentedInstitutionsByUserIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
    )
        : base(
            batchScheduler,
            options,
            dbContextFactory,
            (dbContext, ids) =>
                dbContext.InstitutionRepresentatives.AsQueryable().Where(x =>
                    !x.Pending && ids.Contains(x.UserId)
                ),
            x => x.UserId
        )
    {
    }
}