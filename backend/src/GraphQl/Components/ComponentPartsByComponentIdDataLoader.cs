using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components;

public sealed class ComponentPartsByComponentIdDataLoader
    : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentAssembly>
{
    public ComponentPartsByComponentIdDataLoader(
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
                    ids.Contains(x.AssembledComponentId)
                ).OrderBy(x => x.Index),
            x => x.AssembledComponentId
        )
    {
    }
}