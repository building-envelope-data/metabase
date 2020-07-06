using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

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