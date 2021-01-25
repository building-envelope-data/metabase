using Microsoft.EntityFrameworkCore;
using GreenDonut;
using Metabase.GraphQl.Entitys;

namespace Metabase.GraphQl.Persons
{
    public sealed class PersonByIdDataLoader
      : EntityByIdDataLoader<Data.Person>
    {
        public PersonByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                dbContext => dbContext.Persons
                )
        {
        }
    }
}
