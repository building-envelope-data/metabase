using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class ComponentVersionDataLoader
      : ModelDataLoader<ComponentVersion, Models.ComponentVersion>
    {
        public ComponentVersionDataLoader(IQueryBus queryBus)
          : base(ComponentVersion.FromModel, queryBus)
        {
        }
    }
}