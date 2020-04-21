using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class ComponentsAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Component, Models.Component>
    {
        public ComponentsAtTimestampDataLoader(IQueryBus queryBus)
          : base(Component.FromModel, queryBus)
        {
        }
    }
}