using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class DatabasesAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Database, Models.Database>
    {
        public DatabasesAtTimestampDataLoader(IQueryBus queryBus)
          : base(Database.FromModel, queryBus)
        {
        }
    }
}