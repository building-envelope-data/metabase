using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class ComponentPartDataLoader
      : ModelDataLoader<ComponentPart, Models.ComponentPart>
    {
        public ComponentPartDataLoader(IQueryBus queryBus)
          : base(ComponentPart.FromModel, queryBus)
        {
        }
    }
}