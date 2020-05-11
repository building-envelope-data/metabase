using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class StandardForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<Standard, Models.Standard>
    {
        public StandardForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(Standard.FromModel, queryBus)
        {
        }
    }
}