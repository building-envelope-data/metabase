using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

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