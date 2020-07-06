using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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