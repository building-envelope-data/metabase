using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Methods;

public sealed class UserMethodDevelopersByMethodIdDataLoader
    : AssociationsByAssociateIdDataLoader<UserMethodDeveloper>
{
    public UserMethodDevelopersByMethodIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
        : base(
            batchScheduler,
            options,
            dbContextFactory,
            (dbContext, ids) =>
                dbContext.UserMethodDevelopers.AsQueryable().Where(x =>
                    !x.Pending && ids.Contains(x.MethodId)
                ),
            x => x.MethodId
        )
    {
    }
}