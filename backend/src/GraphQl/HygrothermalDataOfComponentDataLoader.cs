using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class HygrothermalDataOfComponentDataLoader
      : DataOfComponentDataLoader<Models.HygrothermalData, HygrothermalData>
    {
        public HygrothermalDataOfComponentDataLoader(IQueryBus queryBus)
          : base(HygrothermalData.FromModel, queryBus)
        {
        }
    }
}