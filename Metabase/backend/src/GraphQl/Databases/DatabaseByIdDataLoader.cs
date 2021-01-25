using Microsoft.EntityFrameworkCore;
using GreenDonut;
using Metabase.GraphQl.Entitys;

namespace Metabase.GraphQl.Databases
{
    public sealed class DatabaseByIdDataLoader
      : EntityByIdDataLoader<Data.Database>
    {
        public DatabaseByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                dbContext => dbContext.Databases
                )
        {
        }
    }
}
