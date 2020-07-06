using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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