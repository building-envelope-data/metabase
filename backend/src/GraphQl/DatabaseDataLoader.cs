using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class DatabaseDataLoader
      : ModelDataLoader<Database, Models.Database>
    {
        public DatabaseDataLoader(IQueryBus queryBus)
          : base(Database.FromModel, queryBus)
        {
        }
    }
}