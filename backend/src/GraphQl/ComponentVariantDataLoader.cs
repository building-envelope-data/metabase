using Models = Icon.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

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