using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentsByInstitutionIdDataLoader
    : AssociationsByAssociateIdDataLoader<ComponentManufacturer>
{
    public InstitutionManufacturedComponentsByInstitutionIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<ApplicationDbContext> dbContextFactory
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