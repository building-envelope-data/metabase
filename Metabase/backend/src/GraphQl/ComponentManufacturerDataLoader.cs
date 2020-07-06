using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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