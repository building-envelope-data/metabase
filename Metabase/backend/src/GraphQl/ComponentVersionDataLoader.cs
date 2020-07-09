using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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