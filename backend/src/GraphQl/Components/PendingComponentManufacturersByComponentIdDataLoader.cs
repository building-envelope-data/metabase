using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components
{
    public sealed class PendingComponentManufacturersByComponentIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentManufacturer>
    {
        public PendingComponentManufacturersByComponentIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.ComponentManufacturers.AsQueryable().Where(x =>
                        x.Pending && ids.Contains(x.ComponentId)
                    ),
                x => x.ComponentId
                )
        {
        }
    }
}