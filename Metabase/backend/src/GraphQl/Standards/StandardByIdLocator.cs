using GreenDonut;
using Metabase.GraphQl.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Standards
{
    public sealed class StandardByIdDataLoader
      : EntityByIdDataLoader<Data.Standard>
    {
        public StandardByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                dbContext => dbContext.Standards
                )
        {
        }
    }
}