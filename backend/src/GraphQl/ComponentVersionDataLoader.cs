using Models = Icon.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

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