using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class DatabasesAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Database, Models.Database>
    {
        public DatabasesAtTimestampDataLoader(IQueryBus queryBus)
          : base(Database.FromModel, queryBus)
        {
        }
    }
}