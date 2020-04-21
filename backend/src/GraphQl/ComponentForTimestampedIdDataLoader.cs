using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class ComponentForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<Component, Models.Component>
    {
        public ComponentForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(Component.FromModel, queryBus)
        {
        }
    }
}