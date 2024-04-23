using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentsByInstitutionIdDataLoader
    : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentManufacturer>
{
    public InstitutionManufacturedComponentsByInstitutionIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
    )
        : base(
            batchScheduler,
            options,
            dbContextFactory,
            (dbContext, ids) =>
                dbContext.ComponentManufacturers.AsQueryable().Where(x =>
                    !x.Pending && ids.Contains(x.InstitutionId)
                ),
            x => x.InstitutionId
        )
    {
    }
}