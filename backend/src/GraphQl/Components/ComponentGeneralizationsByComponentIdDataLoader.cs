using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components;

public sealed class ComponentGeneralizationsByComponentIdDataLoader
    : AssociationsByAssociateIdDataLoader<ComponentConcretizationAndGeneralization>
{
    public ComponentGeneralizationsByComponentIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
        : base(
            batchScheduler,
            options,
            dbContextFactory,
            (dbContext, ids) =>
                dbContext.ComponentConcretizationAndGeneralizations.AsQueryable().Where(x =>
                    ids.Contains(x.ConcreteComponentId)
                ),
            x => x.ConcreteComponentId
        )
    {
    }
}