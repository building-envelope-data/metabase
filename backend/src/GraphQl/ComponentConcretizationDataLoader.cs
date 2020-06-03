using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class ComponentConcretizationDataLoader
      : ModelDataLoader<ComponentConcretization, Models.ComponentConcretization>
    {
        public ComponentConcretizationDataLoader(IQueryBus queryBus)
          : base(ComponentConcretization.FromModel, queryBus)
        {
        }
    }
}