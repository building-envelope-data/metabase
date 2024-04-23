using GreenDonut;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionByIdDataLoader
        : EntityByIdDataLoader<Data.Institution>
    {
        public InstitutionByIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
        )
            : base(
                batchScheduler,
                options,
                dbContextFactory,
                dbContext => dbContext.Institutions
            )
        {
        }
    }
}