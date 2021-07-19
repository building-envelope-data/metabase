using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class PendingInstitutionManufacturedComponentsByInstitutionIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentManufacturer>
    {
        public PendingInstitutionManufacturedComponentsByInstitutionIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.ComponentManufacturers.AsQueryable().Where(x =>
                        x.Pending && ids.Contains(x.InstitutionId)
                    ),
                x => x.InstitutionId
                )
        {
        }
    }
}