using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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