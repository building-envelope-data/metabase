using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentGeneralizationsByComponentIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.ComponentConcretizationAndGeneralization>
    {
        public ComponentGeneralizationsByComponentIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
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
}