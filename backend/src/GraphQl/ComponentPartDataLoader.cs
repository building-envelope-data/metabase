using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

namespace Icon.GraphQl
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