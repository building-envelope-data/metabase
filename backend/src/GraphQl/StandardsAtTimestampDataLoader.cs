using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class StandardsAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Standard, Models.Standard>
    {
        public StandardsAtTimestampDataLoader(IQueryBus queryBus)
          : base(Standard.FromModel, queryBus)
        {
        }
    }
}