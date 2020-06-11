using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class HygrothermalDataOfComponentDataLoader
      : ForwardOneToManyAssociatesOfModelDataLoader<HygrothermalData, Models.Component, Models.ComponentHygrothermalData, Models.HygrothermalData>
    {
        public HygrothermalDataOfComponentDataLoader(IQueryBus queryBus)
          : base(HygrothermalData.FromModel, queryBus)
        {
        }
    }
}