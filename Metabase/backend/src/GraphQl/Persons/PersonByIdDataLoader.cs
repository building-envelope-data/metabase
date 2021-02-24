using GreenDonut;
using Metabase.GraphQl.Entitys;
using Microsoft.EntityFrameworkCore;

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