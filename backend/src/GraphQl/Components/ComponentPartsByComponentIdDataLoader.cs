using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components;

public sealed class ComponentPartsByComponentIdDataLoader
    : AssociationsByAssociateIdDataLoader<ComponentAssembly>
{
    public ComponentPartsByComponentIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<ApplicationDbContext> dbContextFactory
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