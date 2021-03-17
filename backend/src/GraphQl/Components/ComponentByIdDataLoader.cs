using GreenDonut;
using Metabase.GraphQl.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentByIdDataLoader
      : EntityByIdDataLoader<Data.Component>
    {
        public ComponentByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                dbContext => dbContext.Components
                )
        {
        }
    }
}