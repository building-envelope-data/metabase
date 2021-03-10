using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentManufacturersByComponentIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentManufacturer>
    {
        public ComponentManufacturersByComponentIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.ComponentManufacturers.AsQueryable().Where(x =>
                        ids.Contains(x.ComponentId)
                    ),
                x => x.ComponentId
                )
        {
        }
    }
}