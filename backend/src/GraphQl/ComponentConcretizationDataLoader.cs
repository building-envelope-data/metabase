using Models = Icon.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class ComponentConcretizationForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<ComponentConcretization, Models.ComponentConcretization>
    {
        public ComponentConcretizationForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(ComponentConcretization.FromModel, queryBus)
        {
        }
    }
}