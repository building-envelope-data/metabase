using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
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