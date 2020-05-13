using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class ComponentDataLoader
      : ModelDataLoader<Component, Models.Component>
    {
        public ComponentDataLoader(IQueryBus queryBus)
          : base(Component.FromModel, queryBus)
        {
        }
    }
}