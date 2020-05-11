using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class DatabaseForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<Database, Models.Database>
    {
        public DatabaseForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(Database.FromModel, queryBus)
        {
        }
    }
}