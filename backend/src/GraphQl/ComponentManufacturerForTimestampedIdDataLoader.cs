using Models = Icon.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class ComponentManufacturerForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<ComponentManufacturer, Models.ComponentManufacturer>
    {
        public ComponentManufacturerForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(ComponentManufacturer.FromModel, queryBus)
        {
        }
    }
}