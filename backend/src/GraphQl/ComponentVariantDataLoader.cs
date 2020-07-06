using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class ComponentVariantDataLoader
      : ModelDataLoader<ComponentVariant, Models.ComponentVariant>
    {
        public ComponentVariantDataLoader(IQueryBus queryBus)
          : base(ComponentVariant.FromModel, queryBus)
        {
        }
    }
}