using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

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