using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

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