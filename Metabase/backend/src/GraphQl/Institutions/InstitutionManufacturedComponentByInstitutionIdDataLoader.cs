using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManufacturedComponentsByInstitutionIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentManufacturer>
    {
        public InstitutionManufacturedComponentsByInstitutionIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.ComponentManufacturers.Where(x =>
                        ids.Contains(x.InstitutionId)
                    ),
                x => x.InstitutionId
                )
        {
        }
    }
}