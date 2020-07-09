using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class StandardsAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Standard, Models.Standard>
    {
        public StandardsAtTimestampDataLoader(IQueryBus queryBus)
          : base(Standard.FromModel, queryBus)
        {
        }
    }
}