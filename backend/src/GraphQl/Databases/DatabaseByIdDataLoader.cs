using GreenDonut;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

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