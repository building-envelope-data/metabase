using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDevelopedMethodsByInstitutionIdDataLoader
    : Entities.AssociationsByAssociateIdDataLoader<Data.InstitutionMethodDeveloper>
{
    public InstitutionDevelopedMethodsByInstitutionIdDataLoader(
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
                    !x.Pending && ids.Contains(x.InstitutionId)
                ),
            x => x.InstitutionId
        )
    {
    }
}