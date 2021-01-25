using Microsoft.EntityFrameworkCore;
using GreenDonut;
using Metabase.GraphQl.Entitys;

namespace Metabase.GraphQl.Methods
{
    public sealed class MethodByIdDataLoader
      : EntityByIdDataLoader<Data.Method>
    {
        public MethodByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                dbContext => dbContext.Methods
                )
        {
        }
    }
}
