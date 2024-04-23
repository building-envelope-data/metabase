using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentPartOfByComponentIdDataLoader
        : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentAssembly>
    {
        public ComponentPartOfByComponentIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
        )
            : base(
                batchScheduler,
                options,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.ComponentAssemblies.AsQueryable().Where(x =>
                        ids.Contains(x.PartComponentId)
                    ),
                x => x.PartComponentId
            )
        {
        }
    }
}