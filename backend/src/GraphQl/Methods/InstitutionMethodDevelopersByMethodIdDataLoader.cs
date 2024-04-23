using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Methods;

public sealed class InstitutionMethodDevelopersByMethodIdDataLoader
    : Entities.AssociationsByAssociateIdDataLoader<Data.InstitutionMethodDeveloper>
{
    public InstitutionMethodDevelopersByMethodIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
    )
        : base(
            batchScheduler,
            options,
            dbContextFactory,
            (dbContext, ids) =>
                dbContext.InstitutionMethodDevelopers.AsQueryable().Where(x =>
                    !x.Pending && ids.Contains(x.MethodId)
                ),
            x => x.MethodId
        )
    {
    }
}