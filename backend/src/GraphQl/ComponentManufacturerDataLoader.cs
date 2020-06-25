using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class ComponentManufacturerDataLoader
      : ModelDataLoader<ComponentManufacturer, Models.ComponentManufacturer>
    {
        public ComponentManufacturerDataLoader(IQueryBus queryBus)
          : base(ComponentManufacturer.FromModel, queryBus)
        {
        }
    }
}