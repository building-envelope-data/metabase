using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
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