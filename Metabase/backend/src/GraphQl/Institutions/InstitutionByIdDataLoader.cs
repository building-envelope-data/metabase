using Microsoft.EntityFrameworkCore;
using GreenDonut;
using Metabase.GraphQl.Entitys;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionByIdDataLoader
      : EntityByIdDataLoader<Data.Institution>
    {
        public InstitutionByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                dbContext => dbContext.Institutions
                )
        {
        }
    }
}
