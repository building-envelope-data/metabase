using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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