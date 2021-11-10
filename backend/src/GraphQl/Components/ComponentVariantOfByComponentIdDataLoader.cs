using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentVariantOfByComponentIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentVariant>
    {
        public ComponentVariantOfByComponentIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                options,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.ComponentVariants.AsQueryable().Where(x =>
                        ids.Contains(x.ToComponentId)
                    ),
                x => x.ToComponentId
                )
        {
        }
    }
}