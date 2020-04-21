using Models = Icon.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public class ComponentManufacturerForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<ComponentManufacturer, Models.ComponentManufacturer>
    {
        public ComponentManufacturerForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(ComponentManufacturer.FromModel, queryBus)
        {
        }
    }
}