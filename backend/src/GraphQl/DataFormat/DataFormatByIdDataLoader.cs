using GreenDonut;
using Metabase.GraphQl.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.DataFormats
{
    public sealed class DataFormatByIdDataLoader
      : EntityByIdDataLoader<Data.DataFormat>
    {
        public DataFormatByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                dbContext => dbContext.DataFormats
                )
        {
        }
    }
}