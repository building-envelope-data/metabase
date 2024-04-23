using GreenDonut;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.DataFormats
{
    public sealed class DataFormatByIdDataLoader
        : EntityByIdDataLoader<Data.DataFormat>
    {
        public DataFormatByIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
        )
            : base(
                batchScheduler,
                options,
                dbContextFactory,
                dbContext => dbContext.DataFormats
            )
        {
        }
    }
}