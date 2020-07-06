using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class ComponentsAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Component, Models.Component>
    {
        public ComponentsAtTimestampDataLoader(IQueryBus queryBus)
          : base(Component.FromModel, queryBus)
        {
        }
    }
}